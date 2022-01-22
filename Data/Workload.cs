using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSwitchWeb.Data
{
    public class Workload
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("workloadDefinitionId")]
        public int WorkloadDefinitionId { get; set; }

        [JsonPropertyName("workloadW")]
        public int WorkloadW { get; set; }

        [JsonPropertyName("durationM")]
        public int DurationM { get; set; }

        [JsonPropertyName("toleranceDurationM")]
        public int ToleranceDurationM { get; set; }

        [JsonPropertyName("repeatPattern")]
        public RepeatPattern[] RepeatPattern { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("expiryDate")]
        public System.DateTime ExpiryDate { get; set; }
    }
    public class RepeatPattern
    {
        [JsonPropertyName("monthFlags")]
        public ushort MonthFlags { get; set; }
        [JsonPropertyName("dayFlags")]
        public uint DayFlags { get; set; }
        [JsonPropertyName("hourFlags")]
        public uint HourFlags { get; set; }
        [JsonPropertyName("minuteFlags")]
        public ulong MinuteFlags { get; set; }

        [JsonPropertyName("weekdayFlags")]
        public byte WeekdayFlags { get; set; }
    }

}