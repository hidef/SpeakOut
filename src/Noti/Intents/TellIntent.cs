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
        
        public string Invoke(string Recipient, string Message)
        {
            _client.As<Message>().Lists[Recipient].Add(new Message {
                Id = Guid.NewGuid().ToString(),
                From = this.ctx.UserId,
                Text = Message,
                Sent = DateTimeOffset.Now
            });

            return $"Telling {Recipient} your message.";
        }
    }
}