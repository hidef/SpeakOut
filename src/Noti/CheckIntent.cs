
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Lambda.Core;
using ServiceStack.Redis;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
namespace Noti
{
    public class CheckIntent
    {
        IRedisClient _client;

        public CheckIntent(IRedisClient client)
        {
            _client = client;
        }

        public string Check(string inboxName)
        {
            Message nextMessage = null;
            List<Message> messages = new List<Message>();
            while ( (nextMessage = _client.As<Message>().Lists[inboxName].Dequeue()) != null )
            {
                messages.Add(nextMessage);
            }

            if ( messages.Count == 0 ) return "You have no messages";

            string response = messages
                .GroupBy(m => m.From)
                .Select(g => $"{g.Key} says {g.Select(m => m.Text).Aggregate((a, b) => a + ", and " + b)}")
                .Aggregate((a, b) => ", and ");

            return response;
        }
    }
}
