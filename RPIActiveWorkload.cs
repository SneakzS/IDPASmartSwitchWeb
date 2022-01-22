using System;
using System.Text.Json.Serialization;

namespace SmartSwitchWeb
{
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