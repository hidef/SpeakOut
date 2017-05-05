using System;

namespace Noti
{
    internal class TellIntent
    {
        public TellIntent()
        {
        }

        internal string Tell(string recipient, string message)
        {
            return $"Telling {recipient} your message.";
        }
    }
}