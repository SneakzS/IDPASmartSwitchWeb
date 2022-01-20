using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartSwitchWeb.Data;
using SmartSwitchWeb.Handlers;
using System;

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

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("durationM")]
        public int DurationM { get; set; }

        public enum Action : int
        {
            SetWorkload = 1,
            GetWorkload = 2,
            DeleteWorkload = 3,
            SetFlags = 4,
            GetFlags = 5,
            Helo = 6,
            GetWorkloads = 7,

            NotifyError = 101,
            NotifyWorkloadCreated = 102,
            NotifyNoContent = 103,
            NotifyWorkloads = 104
        }

        public static string Serialize(RPIMessage msg)
        {
            var data = JsonSerializer.Serialize<RPIMessage>(msg);
            var bytes = Encoding.UTF8.GetBytes(data);
            return data;
        }
        public enum Flag
        {
            Enforce = 1 << 0,

            IsEnabled = 1 << 1,

            IsUIConnected = 1 << 2,

            ProviderClientOK = 1 << 3
        }

    }

    public class ActiveWorkload
    {
        [JsonPropertyName("workloadDefinitionId")]
        public int WorkloadDefinitionID { get; set; }

        [JsonPropertyName("offsetM")]
        public int OffsetM { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("durationM")]
        public int DurationM { get; set; }

        [JsonPropertyName("workloadW")]
        public int WorkloadW { get; set; }

    }

}
