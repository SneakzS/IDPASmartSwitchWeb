using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartSwitchWeb.SocketsManager
{
    public abstract class SocketHandler
    {
        public static ConnectionManager Connections { get; set; }
        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }
        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { ConnectionManager.AddSocket(socket); });
        }
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            using (DeviceContext con = new DeviceContext())
            {
                string id = ConnectionManager.GetID(socket);
                await ConnectionManager.RemoveSocketAsync(id);
                Device device = con.GetDevice(id);
                device.SetStatus(DeviceStatus.Offline);
                await con.SaveChangesAsync();
        }

        }
        public static async Task SendMessage(WebSocket socket,string message)
        {
            
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0,message.Length),
                WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }

        public virtual async Task OnConnected(WebSocket socket, string uid)
        {
            await Task.Run(() => { ConnectionManager.AddSocket(socket,uid); });
        }

        public virtual async Task OnChangeID(WebSocket socket, string uid)
        {
            await Task.Run(() => { ConnectionManager.ChangeID(socket, uid); });
        }


        public static async Task SendMessage(string id, string message)
        {
            if (Connections.GetSocketById(id) != null)
            {
                await SendMessage(Connections.GetSocketById(id), message);
            }
            else
            {

            }

        }
        public static async Task SendMessageToAll(string message)
        {
            foreach (var con in ConnectionManager.GetAllConnections())
            {
                await SendMessage(con.Value, message);
            }
        }
        public async void StartForce(bool isEnabled)
        {
            var msg = new RPIMessage
            {
                ActionID = (int)RPIMessage.Action.SetFlags,
                FlagMask = (ulong)(RPIMessage.Flag.IsEnabled | RPIMessage.Flag.Enforce),
                Flags = (ulong)((isEnabled ? RPIMessage.Flag.IsEnabled : 0) | RPIMessage.Flag.Enforce),
            };
            var data = RPIMessage.Serialize(msg);
            await SendMessageToAll(data);
        }
        public async void StartForce(bool isEnabled, string guid)
        {
            var msg = new RPIMessage
            {
                ActionID = (int)RPIMessage.Action.SetFlags,
                FlagMask = (ulong)(RPIMessage.Flag.IsEnabled | RPIMessage.Flag.Enforce),
                Flags = (ulong)((isEnabled ? RPIMessage.Flag.IsEnabled : 0) | RPIMessage.Flag.Enforce),
            };
            var data = RPIMessage.Serialize(msg);
            await SendMessage(guid, data);
        }
        public abstract Task Receive(WebSocket socket,WebSocketReceiveResult result, byte[] buffer);
    }
}
