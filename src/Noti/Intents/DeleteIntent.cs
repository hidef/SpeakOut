using System;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    internal class DeleteIntent
    {
        private IRedisClient client;

        public DeleteIntent(IRedisClient client)
        {
            this.client = client;
        }

        public string Invoke(string mailbox)
        {
            this.client.As<Message>().Lists[mailbox].Clear();
            return "ok";
        }
    }
}