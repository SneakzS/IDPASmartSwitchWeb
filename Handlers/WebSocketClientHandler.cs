using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System;
using SmartSwitchWeb.Database;

namespace SmartSwitchWeb.Handlers
{
    public class WebSocketClientHandler : IWebSocketClientHandler
    {
        private Dictionary<string, WebSocket> _clients;


        public WebSocketClientHandler()
        {
            _clients = new Dictionary<string, WebSocket>();
        }

        byte[] SerializeMessage(RPIMessage msg)
        {
            var data = JsonSerializer.Serialize(msg);
            return Encoding.UTF8.GetBytes(data);
        }

        public async Task Broadcast(RPIMessage message)
        {
            var data = SerializeMessage(message);
            foreach (var socket in _clients.Values)
            {
                await socket.SendAsync(data, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            }
        }

        public async Task HandleClientClosed(WebSocket socket, string clientGUID)
        {
            _clients.Remove(clientGUID);

            using (var ctx = new DeviceContext())
            {
                var device = ctx.GetDevice(clientGUID);
                if (device == null)
                    return;
                device.Status = Data.DeviceStatus.Offline;
                device.LastOnline = DateTime.UtcNow;

                await ctx.SaveChangesAsync();
            }
        }

        public async Task HandleClientMessage(WebSocket socket, string clientGUID, RPIMessage message)
        {
            // ignore client message
        }

        public async Task HandleNewConnection(WebSocket socket, string clientGUID)
        {
            WebSocket oldSocket;
            if (_clients.TryGetValue(clientGUID, out oldSocket))
            {
                try
                {
                    await oldSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "You must only connect once", System.Threading.CancellationToken.None);
                }
                catch
                {
                    // in the bin
                }

            }
            _clients.Add(clientGUID, socket);
            using (var ctx = new DeviceContext())
            {
                var device = ctx.GetDevice(clientGUID);
                if (device == null)
                {
                    device = new Data.Device
                    {
                        Name = "new Device",
                        Description = "My new device",
                        Guid = clientGUID,
                    };
                    ctx.Add(device);
                }

                device.LastOnline = DateTime.UtcNow;
                device.Status = Data.DeviceStatus.Online;

                await ctx.SaveChangesAsync();
            }
        }

        public async Task SendClientMessage(string clientGUID, RPIMessage message,bool isResending = false)
        {
            WebSocket socket;
            var data = SerializeMessage(message);

            using (var ctx = new MessageContext())
            {
                if (_clients.TryGetValue(clientGUID, out socket) && socket.State == WebSocketState.Open)
                { if (isResending == false)
                    {
                        new Data.Message(data, true, clientGUID);
                    }
                    else
                    {
                    }
                    await socket.SendAsync(data, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                }
                else
                {
                    Console.Error.WriteLine("client {0} is not connected", clientGUID);
                    new Data.Message(data, false, clientGUID);
                    // TODO: cache in database and send when connected
                }
            }
        }
    }
}