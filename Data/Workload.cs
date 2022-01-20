using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartSwitchWeb.Data
{
    public class Workload
    {
        public int WorkloadID { get; set; }
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

        public Workload()
        {   
                    
        }
    }
    public class RepeatPattern
    {
        public int RepeatPatternID { get; set; }

        [JsonPropertyName("monthFlags")]
        public ushort MonthFlags { get; set; }
        [JsonPropertyName("dayFlags")]
        public uint DayFlags { get; set; }
        [JsonPropertyName("hourFlags")]
        public uint HourFlags { get; set; }
        [JsonPropertyName("minuteFlags")]
        public ulong MinuteFlags { get; set; }
        [JsonPropertyName("weekdayFlags")]
        public ushort WeekdayFlags { get; set; }
        [JsonPropertyName("expiryDate")]
        public System.DateTime ExpiryDate { get; set; }
        public static uint SetFlag(List<uint> bitPos) 
        {
            uint finalInt = 0;
            foreach(var bit in bitPos)
            {
                finalInt |= ((uint)1) << (int)bit;
            }
            return finalInt;
        }
        public static ulong SetFlagUlong(List<ulong> bitPos)
        {
            ulong finalInt = 0;
            foreach (var bit in bitPos)
            {
                finalInt |= ((ulong)1) << (int)bit;
            }
            return finalInt;
        }
        public static ushort SetFlagUshort(List<ushort> bitPos)
        {
            ushort finalInt = 0;
            foreach (ushort bit in bitPos)
            {
                finalInt |=(ushort)((1) << bit);
            }
            return finalInt;
        }
    }

}