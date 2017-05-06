using System;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    internal class DeleteIntent
    {
        private IRedisClient client;
        private Context ctx;

        public DeleteIntent(Context ctx, IRedisClient client)
        {
            this.client = client;
            this.ctx = ctx;
        }

        public string Invoke()
        {
            this.client.Db = RedisDBs.MailBoxes;
            this.client.As<Message>().Lists[this.ctx.UserId].Clear();
            return "ok";
        }
    }
}