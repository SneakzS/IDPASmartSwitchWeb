using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using SmartSwitchWeb.SocketsManager;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartSwitchWeb.Handlers
{
    
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections): base(connections)
        {

        }
        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Connections.GetID(socket);
            //await SendMessageToAll($"{socketId} joined");
        }
        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            try
            {
                string receiveResult = Encoding.UTF8.GetString(buffer, 0, result.Count);
                RPIMessage rPIMessage = JsonSerializer.Deserialize<RPIMessage>(receiveResult);
                using (DeviceContext con = new DeviceContext())
                {
                    switch (rPIMessage.ActionID)
                    {

                        case (int)RPIMessage.Action.Helo:
                            await OnChangeID(socket, rPIMessage.ClientGUID);
                            Device connectedDevice = con.GetDevice(rPIMessage.ClientGUID);
                            if (connectedDevice != null)
                            {
                                connectedDevice.SetStatus(DeviceStatus.Online);
                            }
                            else
                            {
                                var newdevice = new Device("newDevice", "", rPIMessage.ClientGUID, DeviceStatus.Online);
                                try { con.Add(newdevice); } catch (Exception e) { Console.WriteLine(e); }

                            }
                            await con.SaveChangesAsync();
                            break;

                        default:
                            break;
                    }
                }
            }
            catch
            {

            }
        }

    }
}
