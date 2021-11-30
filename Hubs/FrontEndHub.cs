using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SmartSwitchWeb.SocketsManager;

namespace SmartSwitchWeb.Hubs
{
    public class FrontEndHub : Hub
    {
        public const string HubUrl = "/message";
        public async void TestMessage(bool isEnabled)
        {
            ulong mask;
            if (isEnabled)
            {
                mask = 5;
            }
            else
            {
                mask = 99;
            }
            var msg = new RPIMessage
            {
                ActionID = RPIMessage.ActionSetFlags,
                FlagMask = mask,
                Flags = RPIMessage.FlagIsEnabled
            };
            var data = RPIMessage.ConvertToString(msg);
            var test = new Handlers.WebSocketMessageHandler(SocketsManager.SocketHandler.Connections);
            await test.SendMessageToAll(data);
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}
