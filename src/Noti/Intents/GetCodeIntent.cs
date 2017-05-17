using System;
using ServiceStack.Redis;

namespace SpeakOut.Intents
{
    [Utterance("Give me a code")]
    [Utterance("Give me a friend code")]
    [Utterance("Give me a new friend code")]
    [Utterance("Generate a code")]
    [Utterance("Generate a friend code")]
    [Utterance("I want a code")]
    [Utterance("I want a friend code")]
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
            _client.As<string>().ExpireIn(code, TimeSpan.FromMinutes(5));
            return code;
        }
    }
}