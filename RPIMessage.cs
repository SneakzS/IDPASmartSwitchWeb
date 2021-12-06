using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartSwitchWeb.Handlers;

namespace SmartSwitchWeb
{
    public class RPIMessage
    {
        public const int ActionSetFlags = 4;

        public const ulong FlagEnforce = 1 << 0;
        public const ulong FlagIsEnabled = 1 << 1;
        public const ulong FlagHasError = 1 << 2;

        [JsonPropertyName("actionId")]
        public int ActionID { get; set; }
        [JsonPropertyName("flags")]
        public ulong Flags { get; set; }

        [JsonPropertyName("flagMask")]
        public ulong FlagMask { get; set; }
        public enum Action{
            SetWorkload = 1,
            GetWorkload = 2,
            DeleteWorkload = 3,
            SetFlags = 4,
            GetFlags = 5
        }

        public static string Serialize(RPIMessage msg)
        {
            var data = JsonSerializer.Serialize<RPIMessage>(msg);
            var bytes = Encoding.UTF8.GetBytes(data);
            return data;
        }
       
    }
}
