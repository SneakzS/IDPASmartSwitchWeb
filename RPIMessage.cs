using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartSwitchWeb.Data;
using SmartSwitchWeb.Handlers;

namespace SmartSwitchWeb
{

    public class RPIMessage
    {

        [JsonPropertyName("actionId")]
        public int ActionID { get; set; }
        [JsonPropertyName("flags")]
        public ulong Flags { get; set; }

        [JsonPropertyName("flagMask")]
        public ulong FlagMask { get; set; }
        [JsonPropertyName("clientGuid")]
        public string ClientGUID { get; set; }

        [JsonPropertyName("workloadDefinitions")]
        public Workload[] Workload { get; set; }

        public enum Action : int{
            SetWorkload = 1,
            GetWorkload = 2,
            DeleteWorkload = 3,
            SetFlags = 4,
            GetFlags = 5,
            Helo = 6
        }

        public static string Serialize(RPIMessage msg)
        {
            var data = JsonSerializer.Serialize<RPIMessage>(msg);
            var bytes = Encoding.UTF8.GetBytes(data);
            return data;
        }
        public enum Flag {
            Enforce = 1 << 0,

            IsEnabled = 1 << 1,

            IsUIConnected = 1 << 2,

            ProviderClientOK = 1 << 3
        }

    }

}
