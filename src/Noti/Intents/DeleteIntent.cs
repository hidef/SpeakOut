using System;
using SpeakOut.Models;
using ServiceStack.Redis;

namespace SpeakOut.Intents
{
    [Utterance("Delete my messages")]
    [Utterance("Clear my messages")]
    [Utterance("Delete my inbox")]
    [Utterance("Clear my inbox")]
    internal class DeleteIntent : IntentBase
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