using System;

namespace SmartSwitchWeb.Data
{
    public class Message
    {
        public int MessageID { get; set; }
        public DateTime Time { get; set; }
        public string MessageJSON { get; set; }
        public bool IsSended { get; set; }
        public string Guid { get; set; }
        public bool IsResended { get; set; }

        public Message(string messageJson, bool isSended, string guid)
        {
            MessageJSON = messageJson;
            IsSended = isSended;
            Guid = guid;
            Time = DateTime.Now;
            IsResended = false;
        }
    }
}
