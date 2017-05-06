using System;
using System.Collections.Generic;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{
    internal class ForgetFriendIntent
    {
        IRedisClient _client;
        private Context ctx;

        public ForgetFriendIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }
        
        public string Invoke(string name)
        {
           
            Dictionary<string, string> addressBook = getAddressBook(this.ctx.UserId);
            if ( addressBook.ContainsKey(name) ) {
                addressBook.Remove(name);
                saveAddressBook(this.ctx.UserId, addressBook);
                return "Done";
            }

            return $"I don't know anyone called {name}";
        }

        private void saveAddressBook(string userId, Dictionary<string, string> addressBook)
        {
            _client.Db = RedisDBs.AddressBooks;
            _client.As<Dictionary<string, string>>().SetValue(userId, addressBook);
        }

        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetById(userId);
        }
    }
}