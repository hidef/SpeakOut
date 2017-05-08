using System;
using System.Collections.Generic;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    [Utterance("Tell {jack|recipient} that {I will be home late|message}")]
    [Utterance("Let {jack|recipient} know {I will be home late|message}")]
    [Utterance("Let {jack|recipient} know that {I will be home late|message}")]
    internal class TellIntent : IntentBase
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
            var addressBook = getAddressBook(this.ctx.UserId);
            if ( !addressBook.ContainsKey(recipient) ) return $"I don't know anyone called {recipient}.";

            var friendId = addressBook[recipient];
            saveMessage(friendId, message);

            return $"Telling {recipient} your message.";
        }

        private void saveMessage(string friendId, string message)
        {
            _client.Db = RedisDBs.MailBoxes;
            _client.As<Message>().Lists[friendId].Add(new Message {
                Id = Guid.NewGuid().ToString(),
                From = this.ctx.UserId,
                Text = message,
                Sent = DateTimeOffset.Now
            });
        }

        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetValue(userId) ?? new Dictionary<string, string>();
        }
    }
}