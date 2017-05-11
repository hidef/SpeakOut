using System;
using System.Collections.Generic;
using System.Linq;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    [Utterance("Who is in my phone book")]
    [Utterance("Who can I send messages to")]
    internal class ListFriendsIntent : IntentBase
    {
        IRedisClient _client;
        private Context ctx;

        public ListFriendsIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }
        
        public string Invoke()
        {
            var addressBook = getAddressBook(this.ctx.UserId);

            if (addressBook.Keys.Any()) 
            {
                return string.Join(", ", addressBook.Keys);
            }

            return $"You don't have anyone in your phonebook yet. To send messages, get a friend code and then befriend them with that code.";
        }
        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetValue(userId) ?? new Dictionary<string, string>();
        }
    }
}