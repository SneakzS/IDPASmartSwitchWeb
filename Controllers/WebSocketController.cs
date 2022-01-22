using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using SmartSwitchWeb.Handlers;
using SmartSwitchWeb;
using System.Net.WebSockets;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text;

namespace SmartSwitchWeb.Controllers
{

    [Route("/ws")]
    public class WebSocketController : ControllerBase
    {

        IWebSocketClientHandler _clientHandler;
        IHostApplicationLifetime _hostApplicationLifetime;

        public WebSocketController(IWebSocketClientHandler clientHandler, IHostApplicationLifetime hostApplicationLifetime)
        {
            _clientHandler = clientHandler;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var ws = await HttpContext.WebSockets.AcceptWebSocketAsync())
                    await HandleSocket(ws);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
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
                    // Receive allchunks
                    var messageByes = new List<byte>(4096);
                    WebSocketMessageType msgType;
                    for (; ; )
                    {
                        var rawMessage = await socket.ReceiveAsync(buf, _hostApplicationLifetime.ApplicationStopping);
                        messageByes.AddRange(new ArraySegment<byte>(buf, 0, rawMessage.Count));
                        if (rawMessage.EndOfMessage)
                        {
                            msgType = rawMessage.MessageType;
                            break;
                        }


                    }


                    switch (msgType)
                    {
                        case WebSocketMessageType.Close:
                            //await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, ct);
                            return;

                        case WebSocketMessageType.Text:
                            try
                            {
                                message = JsonSerializer.Deserialize<RPIMessage>(new ArraySegment<byte>(messageByes.ToArray()));
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("invalid message received {0}", ex);
                                continue;
                            }

                            if (clientGUID == null)
                            {
                                if (message.ActionID == RPIAction.Helo.ToID())
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

                await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, null, HttpContext.RequestAborted);
            }

        }
    }
}