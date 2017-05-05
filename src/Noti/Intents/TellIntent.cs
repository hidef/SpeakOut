using System;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    internal class TellIntent
    {
        IRedisClient _client;
        private Context ctx;

        public TellIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }
        
        public string Invoke(string recipient, string message)
        {
            _client.As<Message>().Lists[recipient].Add(new Message {
                Id = Guid.NewGuid().ToString(),
                From = this.ctx.UserId,
                Text = message,
                Sent = DateTimeOffset.Now
            });

            return $"Telling {recipient} your message.";
        }
    }
}