using System;
using ServiceStack.Redis;

namespace Noti.Intents
{
    public class GetCodeIntent : IntentBase
    {
        IRedisClient _client;
        private Context ctx;

        public GetCodeIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }
        
        public string Invoke() {
            string code = getCodeFor(this.ctx.UserId);

            return $"You can use friend code {code}. It will be valid for 5 minutes.";
        }

        private string getCodeFor(string userId)
        {
            _client.Db = RedisDBs.Codes;
            string code = DateTime.Now.Millisecond.ToString();
            _client.As<string>().SetValue(code, userId);
            return code;
        }
    }
}