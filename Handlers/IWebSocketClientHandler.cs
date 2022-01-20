using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SmartSwitchWeb.Handlers {
    public interface IWebSocketClientHandler {
        Task HandleNewConnection(WebSocket socket, string clientGUID);
        Task HandleClientMessage(WebSocket socket, string clientGUID, RPIMessage message);
        Task HandleClientClosed(WebSocket socket, string clientGUID);

        Task SendClientMessage(string clientGUID, RPIMessage message, bool isResending = false);
        Task Broadcast(RPIMessage message);
    }
}