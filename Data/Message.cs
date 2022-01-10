using System;

namespace SmartSwitchWeb.Data
{
    public class Message
    {
        public int MessageID { get; set; }
        public DateTime Time { get; set; }
        public string MessageJson { get; set; }
        public bool IsSent { get; set; }
        public string Guid { get; set; }
        public bool IsResent { get; set; }
        public virtual Device Device { get; set; }

        public Message() { }
        public Message(string message, bool isSent, string guid)
        {
            this.MessageJson = message;
            this.IsSent = isSent;
            this.Guid = guid;
            this.Time = DateTime.Now;
            
        }
    }
}
