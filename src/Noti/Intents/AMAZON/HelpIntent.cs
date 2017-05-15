using Noti.Models;

namespace Noti.Intents.AMAZON
{
    public class HelpIntent : IntentBase
    {
        Context ctx;
        
        public HelpIntent(Context ctx)
        {
            this.ctx = ctx;
        }

        public string Invoke()
        {
            return @"Noti lets you send quick messages to friends and family. Ask Noti for a friend code and exchange it with your friend, the ask Noti to befriend them with their code to get set up.
            Then you can ask Noti to tell your friend anything you want. Then you can check for new messages every so often to keep in touch. Lastly if you have lots of messages you don't want, 
            or have friends you no longer want to remeber, you can ask Noti to forget them.";
        }
    }
}
