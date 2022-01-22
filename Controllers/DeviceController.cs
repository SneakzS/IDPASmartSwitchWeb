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

namespace SmartSwitchWeb.Controllers
{
    [Route("/api/v1/devices")]
    public class DeviceController : Controller
    {
        IWebSocketClientHandler _clientHandler;

        public DeviceController(IWebSocketClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
        }

        [HttpGet]
        public IActionResult GetDevices()
        {
            using (var ctx = new DeviceContext())
            {
                var devices = ctx.GetAll().Select(d => FrontendDevice.FromDevice(d));
                return Json(devices);
            }
        }

        [HttpGet]
        [Route("{clientGuid}")]
        public IActionResult GetDevice(string clientGuid)
        {
            using (var ctx = new DeviceContext())
            {
                var dev = ctx.GetDevice(clientGuid);
                if (dev == null)
                    return NotFound();

                return Json(dev);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetDevice()
        {
            var frontendDevice = await JsonSerializer.DeserializeAsync<FrontendDevice>(Request.Body);
            using (var ctx = new DeviceContext())
            {
                var device = ctx.Get(frontendDevice.ID);
                frontendDevice.UpdateDevice(device);
                await ctx.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDevice()
        {
            var frontendDevice = await JsonSerializer.DeserializeAsync<FrontendDevice>(Request.Body);
            using (var ctx = new DeviceContext())
            {
                ctx.Delete(frontendDevice.ID);
                await ctx.SaveChangesAsync();
            }
            return NoContent();
        }


        struct SwitchPosition
        {
            [JsonPropertyName("position")]
            public string Position { get; set; }
        }

        [Route("switch/{clientGuid}")]
        public async Task<IActionResult> Switch(string clientGuid)
        {
            var pos = await JsonSerializer.DeserializeAsync<SwitchPosition>(Request.Body);
            RPIFlag flags;
            RPIFlag flagMask;
            switch (pos.Position)
            {
                case "force-on":
                    flags = RPIFlag.Enforce | RPIFlag.IsEnabled;
                    flagMask = RPIFlag.Enforce | RPIFlag.IsEnabled;
                    break;
                case "force-off":
                    flags = RPIFlag.Enforce;
                    flagMask = RPIFlag.Enforce | RPIFlag.IsEnabled;
                    break;
                case "resume-schedule":
                    flags = 0;
                    flagMask = RPIFlag.Enforce;
                    break;
                default:
                    return BadRequest();
            }

            try
            {
                await _clientHandler.SendClientMessage(clientGuid, new RPIMessage
                {
                    ActionID = RPIAction.SetFlags.ToID(),
                    FlagMask = (ulong)flagMask,
                    Flags = (ulong)flags,
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            return NoContent();
        }
    }
}