using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System;
using SmartSwitchWeb.Database;
using System.Threading;

namespace SmartSwitchWeb.Handlers
{
    public class WebSocketClientHandler : IWebSocketClientHandler
    {
        private Dictionary<string, WebSocket> _clients;
        private Dictionary<int, TaskCompletionSource<RPIMessage>> _requests;

        int _requestIDCount;

        public WebSocketClientHandler()
        {
            _clients = new Dictionary<string, WebSocket>();
            _requests = new Dictionary<int, TaskCompletionSource<RPIMessage>>();
        }

        private int GetNextRequestID()
        {
            lock (this)
            {
                _requestIDCount++;
                return _requestIDCount;
            }
        }

        static byte[] SerializeMessage(RPIMessage msg)
        {
            var data = JsonSerializer.Serialize(msg);
            return Encoding.UTF8.GetBytes(data);
        }

        public async Task Broadcast(RPIMessage message)
        {
            var data = SerializeMessage(message);
            Task[] allTasks;

            lock (_clients)
            {
                allTasks = new Task[_clients.Values.Count];
                int i = 0;
                foreach (var socket in _clients.Values)
                {
                    allTasks[i] = socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                    i++;
                }
            }

            await Task.WhenAll(allTasks);
        }

        public async Task HandleClientClosed(WebSocket socket, string clientGUID)
        {
            lock (_clients)
            {
                _clients.Remove(clientGUID);
            }


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
            // if the message is a response, complete the correspinding request
            if (message.ActionID > 100)
            {
                TaskCompletionSource<RPIMessage> cs;
                bool hasRequest;
                lock (_requests)
                {
                    hasRequest = _requests.TryGetValue(message.RequestID, out cs);
                    if (hasRequest)
                    {
                        _requests.Remove(message.RequestID);
                    }
                }

                if (hasRequest)
                {
                    if (message.ActionID == RPIAction.NotifyError.ToID()) {
                        cs.SetException(new Exception($"request failed: {message.ErrorMessage}"));
                    } else {
                        cs.SetResult(message);
                    }
                    
                }
            }

            // ignore client message
        }

        public async Task HandleNewConnection(WebSocket socket, string clientGUID)
        {
            WebSocket oldSocket;
            bool hasOldSocket;
            lock (_clients)
            {
                hasOldSocket = _clients.TryGetValue(clientGUID, out oldSocket);
                _clients.Add(clientGUID, socket);
            }

            if (hasOldSocket)
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

        public Task<RPIMessage> SendClientRequest(string clientGUID, RPIMessage msg)
        {
            var requestID = GetNextRequestID();
            msg.RequestID = requestID;
            var data = SerializeMessage(msg);
            var cs = new TaskCompletionSource<RPIMessage>();

            WebSocket socket;
            bool hasSocket;
            lock (_clients)
            {
                hasSocket = _clients.TryGetValue(clientGUID, out socket);
            }

            if (hasSocket)
            {
                lock (_requests)
                {
                    _requests[requestID] = cs;
                }

                socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None)
                    .ContinueWith(t =>
                    {

                        if (t.IsCanceled)
                        {
                            cs.SetCanceled();
                        }
                        else if (t.IsFaulted)
                        {
                            // if a send exception occures, remove the request and fail the result
                            lock (_requests)
                            {
                                _requests.Remove(requestID);
                                cs.SetException(t.Exception);
                            }
                        }

                        // in case the sending succeeded, the response will trigger cs to complete

                    });
            }
            else
            {
                cs.SetException(new Exception($"Client with GUID {clientGUID} is not connected"));
            }

            return cs.Task;

        }

        public async Task SendClientMessage(string clientGUID, RPIMessage message, bool isResending = false)
        {
            WebSocket socket;
            var data = SerializeMessage(message);

            using (var ctx = new MessageContext())
            {
                if (_clients.TryGetValue(clientGUID, out socket) && socket.State == WebSocketState.Open)
                {
                    if (isResending == false)
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