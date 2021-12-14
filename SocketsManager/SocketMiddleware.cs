using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using SmartSwitchWeb.Handlers;

namespace SmartSwitchWeb.SocketsManager
{
    public class SocketMiddleware
    {
        private readonly IWebSocketClientHandler _clientHandler;

        public SocketMiddleware(IWebSocketClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/ws")
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleSocket(socket);

            }
            else
            {
                await next(context);
            }

            /*var socket = await context.WebSockets.AcceptWebSocketAsync();
            await Handler.OnConnected(socket);
            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await Handler.OnDisconnected(socket);
                }
            });*/

        }



        async Task HandleSocket(WebSocket socket)
        {
            string clientGUID = null;
            var buf = new byte[4096];
            RPIMessage message;
            try
            {
                for (; ; )
                {
                    var rawMessage = await socket.ReceiveAsync(buf, CancellationToken.None);

                    switch (rawMessage.MessageType)
                    {
                        case WebSocketMessageType.Close:
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                            return;

                        case WebSocketMessageType.Text:
                            try
                            {
                                message = JsonSerializer.Deserialize<RPIMessage>(new ArraySegment<byte>(buf, 0, rawMessage.Count));
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("invalid message received {0}", ex);
                                continue;
                            }

                            if (clientGUID == null)
                            {
                                if (message.ActionID == (int)RPIMessage.Action.Helo)
                                {
                                    clientGUID = message.ClientGUID;
                                    await _clientHandler.HandleNewConnection(socket, clientGUID);
                                }
                                else
                                {
                                    Console.Error.WriteLine("ignoring message. Sender is not authenticated");
                                }
                            }
                            else
                            {
                                await _clientHandler.HandleClientMessage(socket, clientGUID, message);
                            }
                            break;

                    }



                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("error in web socket handler {0}", ex);
            }
            finally
            {
                if (clientGUID != null)
                    await _clientHandler.HandleClientClosed(socket, clientGUID);
                
                await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, null, CancellationToken.None);
            }

        }

    }
}
