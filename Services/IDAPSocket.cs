using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartSwitchWeb.Services
{
    public class IDAPSocket
    {
        public static bool _newData = false;
        public static List<string> MESSAGELIST = new List<String>();

        /*public struct MessageJSON
        {
            int messageType;
            string data;
        }
        private struct NewOrderJSON
        {
            int messageID;
            DateTime firstStartTime;
            DateTime lastEndTime;
            int usageTimeInMin;
            bool startNow;

        }
        */
        public static async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                {
                    
                }
                /*
                if (MESSAGELIST.Count != 0)
                {
                    for (int i = 0; i < MESSAGELIST.Count; i++)
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(MESSAGELIST[i].ToString())), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        MESSAGELIST.RemoveAt(i);
                    }
                }*/
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        public static string Pong()
        {
            return "huan";
        }
        public static void CreateJSON()
        {
            int rNumber = new Random().Next();
            Welcome test = new Welcome(msgid: rNumber, isRunning: true);
            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(test);
            MESSAGELIST.Add(serialized);
            
        }

    }
}
