using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ical.Net.DataTypes;
using Microsoft.AspNetCore.SignalR;
using SmartSwitchWeb.Data;
using SmartSwitchWeb.SocketsManager;

namespace SmartSwitchWeb.Hubs
{
    public class FrontEndHub : Hub
    {
        SmartSwitchWeb.Handlers.WebSocketMessageHandler webSocketConnection = new Handlers.WebSocketMessageHandler(SocketsManager.SocketHandler.Connections);
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
            await webSocketConnection.SendMessageToAll(data);
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
        public HashSet<Occurrence> getAppointments()
        {
            return IcalCalendar.GetOccurrences(DateTime.Now, DateTime.Now.AddDays(5));
        }
        public async void AddEvent(DateTime dateTime, DateTime endTime, RecurrencePattern recurrencePattern)
        {

            await webSocketConnection.SendMessageToAll("throw");
        }
    }
}
