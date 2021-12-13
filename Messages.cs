using System;

namespace SmartSwitchWeb
{
    public class Message
    {
        public DateTime time;
        public string message;
        public bool isSended;
        public string guid;
        public bool isResended;

        public Message(string message, bool isSended, string guid)
        {
            this.message = message;
            this.isSended = isSended;
            this.guid = guid;
            this.time = DateTime.Now;
        }
    }
}
