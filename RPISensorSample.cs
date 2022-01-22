using System;
using System.Text.Json.Serialization;

namespace IDPASmartSwitchWeb {
    public struct RPISensorSample {
        [JsonPropertyName("sampleTime")]
        public DateTime SampleTime { get; set; }
        
        [JsonPropertyName("power")]
        public double Power { get; set; }

        [JsonPropertyName("current")]
        public double Current { get; set; }

        [JsonPropertyName("voltage")]
        public double Voltage { get; set; }
    }
}