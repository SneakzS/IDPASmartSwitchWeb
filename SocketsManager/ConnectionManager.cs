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
        private static ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;
        }
        public static ConcurrentDictionary<string,WebSocket> GetAllConnections()
        {
            return _connections;
        }
        public static string GetID(WebSocket socket)
        {
            return _connections.FirstOrDefault(x=>x.Value == socket).Key;
        }
        public static void ChangeID(WebSocket socket, string newID)
        {
            string key = _connections.FirstOrDefault(x => x.Value == socket).Key;
            if(!_connections.Any(x => x.Key == newID))
            {
                Console.WriteLine("{0} renamed to {1}", key ,newID);
                AddSocket(socket, newID);
                RemoveKey(key);
            }
            else
            {
                Console.WriteLine("{0} already exists",newID);
                RemoveKey(key);
                AddSocket(socket, newID);
            }

            
        }
        public static void RemoveKey(string id)
        {
            _connections.TryRemove(id, out WebSocket socket);
        }
        public static async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed",System.Threading.CancellationToken.None);
        }
        public static void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionID(), socket);
        }
        private static string GetConnectionID()
        {
            return Guid.NewGuid().ToString("N");
        }

        internal static void AddSocket(WebSocket socket, string uid)
        {
            _connections.TryAdd(uid, socket);
        }
    }
}
