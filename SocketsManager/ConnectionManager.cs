using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SmartSwitchWeb.SocketsManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;
        }
        public ConcurrentDictionary<string,WebSocket> GetAllConnections()
        {
            return _connections;
        }
        public string GetID(WebSocket socket)
        {
            return _connections.FirstOrDefault(x=>x.Value == socket).Key;
        }
        public async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed",System.Threading.CancellationToken.None);
        }
        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionID(), socket);
        }
        private string GetConnectionID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
