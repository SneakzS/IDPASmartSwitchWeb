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
using System.Linq;
using System.Text.Json.Serialization;
using IDPASmartSwitchWeb;
using System.Text;

namespace SmartSwitchWeb.Controllers
{
    [Route("/api/v1/sensor")]
    public class SensorController : Controller
    {
        IWebSocketClientHandler _clientHandler;
        public SensorController(IWebSocketClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
        }

        async Task<RPISensorSample[]> GetSensorSamplesFromDevice(string clientGuid)
        {
            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.GetSensorSamples.ToID(),
            });
            return resp.SensorSamples;
        }

        [Route("{clientGuid}")]
        public async Task<IActionResult> GetSensorSamples(string clientGuid)
        {

            var samples = await GetSensorSamplesFromDevice(clientGuid);
            return Json(samples);
        }

        static byte[] newLine = Encoding.UTF8.GetBytes("\r\n");

        private async Task WriteBodyLineString(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            await Response.Body.WriteAsync(newLine, 0, newLine.Length);
        }

        const string timeFormat = "yyyy-MM-dd HH:mm:ss";

        [Route("{clientGuid}/csv")]
        public async Task GetSensorSamplesCSV(string clientGuid)
        {
            var samples = await GetSensorSamplesFromDevice(clientGuid);
            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "text/csv");
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"idpa-sensor-{DateTime.UtcNow.ToString(timeFormat)}.csv\"");
            await WriteBodyLineString("sep=;");
            await WriteBodyLineString("Sample Time; Power; Current; Voltage");

            foreach (var sample in samples)
            {
                await WriteBodyLineString($"{sample.SampleTime.ToString(timeFormat)};{sample.Power};{sample.Current};{sample.Voltage}");
            }
        }
    }
}