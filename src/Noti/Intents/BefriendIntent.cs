using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace Noti.Intents
{
    public class BefriendIntent
    {
        IRedisClient _client;
        private Context ctx;

        public BefriendIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }

        // Befriend {name} with code {code}
        public string Invoke(string name, string code) {

            Dictionary<string, string> addressBook = getAddressBook(this.ctx.UserId);
            if ( addressBook.ContainsKey(name) ) {
                return "You already have a friend called {name}, try a different name so I can tell them apart";
            }

            addressBook[name] = getUserIdFromCode(code);

            return $"I now know {name}"; 
        }

        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetById(userId);
        }

        private string getUserIdFromCode(string code)
        {
            _client.Db = RedisDBs.Codes;
            return _client.As<string>().GetById(code);
        }
    }
}