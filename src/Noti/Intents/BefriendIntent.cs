using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace SpeakOut.Intents
{    
    [Utterance("Befriend {jack|name} with code {code}")]
    [Utterance("Add {jack|name} as my friend with code {code}")]
    [Utterance("Add friend {jack|name} with code {code}")]
    [Utterance("Befriend {jack|name}, his code is {code}")]
    [Utterance("Befriend {jill|name}, her code is {code}")]
    [Utterance("Befriend {alex|name}, their code is {code}")]
    public class BefriendIntent : IntentBase
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
            if ( string.IsNullOrEmpty(name) ) return "I didn't catch that name.";
            if ( string.IsNullOrEmpty(name) ) return "I didn't catch that code.";

            Dictionary<string, string> addressBook = getAddressBook(this.ctx.UserId);
            if ( addressBook.ContainsKey(name) ) {
                return $"You already have a friend called {name}, try a different name so I can tell them apart";
            }

            string friendUserId = getUserIdFromCode(code);

            if ( string.IsNullOrEmpty(friendUserId) )
            {
                return "I don't recognise that code. Can you say it again, or get another code?";
            }

            addressBook[name] = friendUserId;

            Console.WriteLine($"{name}:{friendUserId}");

            saveAddressBook(this.ctx.UserId, addressBook);

            return $"I now know {name}"; 
        }

        private void saveAddressBook(string userId, Dictionary<string, string> addressBook)
        {
            _client.Db = RedisDBs.AddressBooks;
            _client.As<Dictionary<string, string>>().SetValue(userId, addressBook);
        }

        private Dictionary<string, string> getAddressBook(string userId)
        {
            _client.Db = RedisDBs.AddressBooks;
            return _client.As<Dictionary<string, string>>().GetValue(userId) ?? new Dictionary<string, string>();
        }

        private string getUserIdFromCode(string code)
        {
            _client.Db = RedisDBs.Codes;
            return _client.As<string>().GetValue(code);
        }
    }
}