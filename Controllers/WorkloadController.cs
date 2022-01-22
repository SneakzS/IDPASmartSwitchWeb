using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;

using SmartSwitchWeb.Data;
using SmartSwitchWeb.Database;
using SmartSwitchWeb.Handlers;
using SmartSwitchWeb;
using System.Linq;
using System.Text.Json.Serialization;

namespace IDPASmartSwitchWeb.Controllers
{

    [Route("/api/v1/workloads")]
    public class WorkloadController : Controller
    {
        IWebSocketClientHandler _clientHandler;


        public WorkloadController(IWebSocketClientHandler clientHandler)
        {
            _clientHandler = clientHandler;
        }

        struct FrontendRepeatPattern
        {
            [JsonPropertyName("monthFlags")]
            public ushort MonthFlags { get; set; }
            [JsonPropertyName("dayFlags")]
            public uint DayFlags { get; set; }
            [JsonPropertyName("hourFlags")]
            public uint HourFlags { get; set; }
            [JsonPropertyName("minuteFlagsLow")]
            public uint MinuteFlagsLow { get; set; }

            [JsonPropertyName("minuteFlagsHigh")]
            public uint MinuteFlagsHigh { get; set; }

            [JsonPropertyName("weekdayFlags")]
            public byte WeekdayFlags { get; set; }

            public static FrontendRepeatPattern FromRepeatPattern(RepeatPattern rp)
            {
                return new FrontendRepeatPattern
                {
                    MonthFlags = rp.MonthFlags,
                    DayFlags = rp.DayFlags,
                    HourFlags = rp.HourFlags,
                    MinuteFlagsLow = (uint)(rp.MinuteFlags & 0xFFFFFFFF),
                    MinuteFlagsHigh = (uint)(rp.MinuteFlags >> 31),
                    WeekdayFlags = rp.WeekdayFlags,
                };
            }

            public RepeatPattern ToRepeatPattern()
            {
                return new RepeatPattern
                {
                    MonthFlags = MonthFlags,
                    DayFlags = DayFlags,
                    HourFlags = HourFlags,
                    MinuteFlags = (ulong)(MinuteFlagsHigh << 31) | (ulong)MinuteFlagsLow,
                    WeekdayFlags = WeekdayFlags,
                };
            }
        }

        struct FrontendWorkload
        {
            [JsonPropertyName("workload")]
            public Workload Workload { get; set; }

            [JsonPropertyName("repeatPattern")]
            public FrontendRepeatPattern[] RepeatPattern { get; set; }

            public static FrontendWorkload FromWorkload(Workload wl)
            {

                return new FrontendWorkload
                {
                    Workload = new Workload
                    {
                        Description = wl.Description,
                        WorkloadDefinitionId = wl.WorkloadDefinitionId,
                        WorkloadW = wl.WorkloadW,
                        DurationM = wl.DurationM,
                        ToleranceDurationM = wl.ToleranceDurationM,
                        IsEnabled = wl.IsEnabled,
                        ExpiryDate = wl.ExpiryDate,
                    },
                    RepeatPattern = wl.RepeatPattern?.Select(rp => FrontendRepeatPattern.FromRepeatPattern(rp)).ToArray(),
                };
            }

            public Workload ToWorkload()
            {
                return new Workload
                {
                    Description = Workload.Description,
                    WorkloadDefinitionId = Workload.WorkloadDefinitionId,
                    WorkloadW = Workload.WorkloadW,
                    DurationM = Workload.DurationM,
                    ToleranceDurationM = Workload.ToleranceDurationM,
                    IsEnabled = Workload.IsEnabled,
                    ExpiryDate = Workload.ExpiryDate,
                    RepeatPattern = RepeatPattern?.Select(rp => rp.ToRepeatPattern()).ToArray(),
                };
            }
        }

        [HttpGet]
        [Route("{clientGuid}")]
        public async Task<IActionResult> GetWorkloads(string clientGuid)
        {
            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.GetWorkloads.ToID(),
            });

            return Json(resp.CurrentWorkloads?.Select(wl => FrontendWorkload.FromWorkload(wl)));
        }

        [HttpPost]
        [Route("{clientGuid}")]
        public async Task<IActionResult> SetWorkload(string clientGuid)
        {
            var wl = await JsonSerializer.DeserializeAsync<FrontendWorkload>(Request.Body);

            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.SetWorkload.ToID(),
                Workload = wl.ToWorkload(),
            });

            return NoContent();
        }

        [HttpDelete]
        [Route("{clientGuid}")]
        public async Task<IActionResult> DeleteWorkload(string clientGuid)
        {
            var wl = await JsonSerializer.DeserializeAsync<FrontendWorkload>(Request.Body);
            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.DeleteWorkload.ToID(),
                Workload = wl.ToWorkload(),
            });

            return NoContent();
        }

        [HttpGet]
        [Route("events/{clientGuid}")]
        public async Task<IActionResult> GetWorkloadEvents(string clientGuid)
        {

            var resp = await _clientHandler.SendClientRequest(clientGuid, new RPIMessage
            {
                ActionID = RPIAction.GetWorkloadEvents.ToID(),
                StartTime = DateTime.UtcNow.AddDays(-30),
                DurationM = 60 * 24 * 180, // next 180 days
            });

            return Json(resp.ActiveWorkloads);
        }
    }
}