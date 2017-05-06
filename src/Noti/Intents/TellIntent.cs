using System;
using System.Collections.Generic;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents
{

    public static class RedisDBs
    {
        public static int MailBoxes = 0;
        public static int Codes = 1;
        public static int AddressBooks = 2;
    }

    internal class TellIntent
    {
        IRedisClient _client;
        private Context ctx;

        public TellIntent(Context ctx, IRedisClient client)
        {
            _client = client;
            this.ctx = ctx;
        }
        
        public string Invoke(string Recipient, string Message)
        {
            var friendId = getAddressBook(this.ctx.UserId)[Recipient];
            saveMessage(friendId, Message);

            return $"Telling {Recipient} your message.";
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
            return _client.As<Dictionary<string, string>>().GetById(userId);
        }
    }
}