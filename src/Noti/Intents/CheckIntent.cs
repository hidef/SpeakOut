
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Lambda.Core;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    [Utterance("Check for messages")]
    [Utterance("Check for new messages")]
    [Utterance("Do I have messages")]
    [Utterance("Do I have any messages")]
    [Utterance("Do I have new messages")]
    [Utterance("Have I got any messages")]
    [Utterance("Check my messages")]
    public class CheckIntent : IntentBase
    {
        IRedisClient _client;
        Context ctx;
        
        public CheckIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }

        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetValue(userId) ?? new Dictionary<string, string>();
        }

        public string Invoke()
        {
            var addressBook = getAddressBook(this.ctx.UserId);

            _client.Db = RedisDBs.MailBoxes;
            Message nextMessage = null;
            List<Message> messages = new List<Message>();
            while ( (nextMessage = _client.As<Message>().Lists[this.ctx.UserId].Dequeue()) != null )
            {
                messages.Add(nextMessage);
            }

            if ( messages.Count == 0 ) return "You have no messages";

            string response = messages
                .GroupBy(m => m.From)
                .Where(g => addressBook.ContainsKey(g.Key))
                .Select(g => $"{addressBook[g.Key]} says {g.Select(m => m.Text).Aggregate((a, b) => a + ", and " + b)}")
                .Aggregate((a, b) => ", and ");

            return response;
        }
    }
}
