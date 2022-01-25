using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System.Text;

using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using SmartSwitchWeb.Handlers;
using SmartSwitchWeb;
using System.Net.WebSockets;
using System.Linq;
using System.Text.Json.Serialization;

namespace SmartSwitchWeb.Controllers
{
    [Route("/api/v1/logs")]
    public class LogController : Controller
    {
        IWebSocketClientHandler _clientHandler;

        public LogController(IWebSocketClientHandler clinetHandler)
        {
            _clientHandler = clinetHandler;
        }

        async Task<RPILogEntry[]> GetLogEntriesFromDevice(string clientGuid)
        {
            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.GetLogEntries.ToID(),
            });
            return resp.LogEntries;
        }

        static byte[] newLine = Encoding.UTF8.GetBytes("\r\n");
        const string timeFormat = "yyyy-MM-dd HH:mm:ss";

        private async Task WriteBodyLineString(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            await Response.Body.WriteAsync(newLine, 0, newLine.Length);
        }

        [Route("{clientGuid}/csv")]
        public async Task GetLogEntriesCSV(string clientGuid)
        {
            var entries = await GetLogEntriesFromDevice(clientGuid);
            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "text/csv");
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"idpa-log-{DateTime.UtcNow.ToString(timeFormat)}.csv\"");
            await WriteBodyLineString("sep=;");
            await WriteBodyLineString("Log Time; Severity; Source; Message");

            foreach (var entry in entries)
            {
                await WriteBodyLineString($"{entry.LogTime.ToString(timeFormat)};{entry.Severity};{entry.Source};{entry.Message}");
            }
        }

    }
}