using System;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    internal class TellIntent
    {
        IRedisClient _client;

        public TellIntent(IRedisClient client)
        {
            _client = client;
        }
        
        public string Invoke(string me, string recipient, string message)
        {
            _client.As<Message>().Lists[recipient].Add(new Message {
                Id = Guid.NewGuid().ToString(),
                From = me,
                Text = message,
                Sent = DateTimeOffset.Now
            });

            return $"Telling {recipient} your message.";
        }
    }
}