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
            await Task.Run(() => { Connections.AddSocket(socket); });
        }
        public virtual async Task OnDisconnected(WebSocket socket)
        {
            using (DeviceContext con = new DeviceContext())
            {
                string id = Connections.GetID(socket);
                await Connections.RemoveSocketAsync(id);
                Device device = con.GetDevice(id);
                device.SetStatus(DeviceStatus.Offline);
                await con.SaveChangesAsync();
        }

        }
        public async Task SendMessage(WebSocket socket,string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0,message.Length),
                WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }

        public virtual async Task OnConnected(WebSocket socket, string uid)
        {
            await Task.Run(() => { Connections.AddSocket(socket,uid); });
        }

        public virtual async Task OnChangeID(WebSocket socket, string uid)
        {
            await Task.Run(() => { Connections.ChangeID(socket, uid); });
        }


        public async Task SendMessage(string id, string message)
        {
            await SendMessage(Connections.GetSocketById(id), message);
        }
        public async Task SendMessageToAll(string message)
        {
            foreach (var con in Connections.GetAllConnections())
            {
                await SendMessage(con.Value, message);
            }
        }
        public abstract Task Receive(WebSocket socket,WebSocketReceiveResult result, byte[] buffer);
    }
}
