using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SmartSwitchWeb.Hubs
{
    public class FrontEndHub : Hub
    {
        public const string HubUrl = "/message";
        public async Task TestMessage(string message, string user)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
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
