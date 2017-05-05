using System;

namespace Noti
{
    public class Message 
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Sent { get; set; }
    }
}