﻿using SmartSwitchWeb.SocketsManager;
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
            var socketId = Connections.GetID(socket);
            try
            {
                string receiveResult = Encoding.UTF8.GetString(buffer, 0, result.Count);
                RPIMessage rPIMessage = JsonSerializer.Deserialize<RPIMessage>(receiveResult);
                switch (rPIMessage.ActionID)
                {
                    case (int)RPIMessage.Action.Helo:
                        await OnChangeID(socket, rPIMessage.ClientGUID);
                        break;

                    default:
                        break;
                }
            }
            catch
            {

            }
        }

    }
}
