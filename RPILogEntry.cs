using System;
using System.Text.Json.Serialization;

namespace SmartSwitchWeb
{
    public class RPILogEntry
    {
        [JsonPropertyName("logId")]
        public int LogID { get; set; }

        [JsonPropertyName("logTime")]
        public DateTime LogTime { get; set; }

        [JsonPropertyName("severity")]
        public int Severity { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}