
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Lambda.Core;
using Noti.Models;
using ServiceStack.Redis;

namespace Noti.Intents.AMAZON
{
    public class CancelIntent : IntentBase
    {
        Context ctx;
        
        public CancelIntent(Context ctx)
        {
            this.ctx = ctx;
        }

        public string Invoke()
        {
            return "";
        }
    }
}
