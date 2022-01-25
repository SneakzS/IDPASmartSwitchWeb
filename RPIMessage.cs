using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartSwitchWeb.Data;
using SmartSwitchWeb.Handlers;
using System;
using IDPASmartSwitchWeb;

namespace SmartSwitchWeb
{

    public class RPIMessage
    {

        [JsonPropertyName("actionId")]
        public int ActionID { get; set; }

        [JsonPropertyName("requestId")]
        public int RequestID { get; set; }

        [JsonPropertyName("workloadDefinition")]
        public Workload Workload { get; set; }

        [JsonPropertyName("flags")]
        public ulong Flags { get; set; }

        [JsonPropertyName("flagMask")]
        public ulong FlagMask { get; set; }
        [JsonPropertyName("clientGuid")]
        public string ClientGUID { get; set; }

        [JsonPropertyName("activeWorkloads")]
        public ActiveWorkload[] ActiveWorkloads { get; set; }

        [JsonPropertyName("currentWorkloadDefinitions")]
        public Workload[] CurrentWorkloads { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("durationM")]
        public int DurationM { get; set; }

        [JsonPropertyName("sensorSamples")]
        public RPISensorSample[] SensorSamples { get; set; }

        [JsonPropertyName("logEntries")]
        public RPILogEntry[] LogEntries { get; set; }
    }



}
