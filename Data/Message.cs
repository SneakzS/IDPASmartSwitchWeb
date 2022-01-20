using System;

namespace SmartSwitchWeb.Data
{
    public class Message
    {
        public int MessageID { get; set; }
        public DateTime Time { get; set; }
        public byte[] MessageJson { get; set; }
        public bool IsSent { get; set; }
        public string Guid { get; set; }
        public virtual Device Device { get; set; }

        public Message() { }
        public Message(byte[] message, bool isSent, string guid)
        {
            this.MessageJson = message;
            this.IsSent = isSent;
            this.Guid = guid;
            this.Time = DateTime.Now;
            
        }
    }
}
