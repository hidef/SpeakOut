using SpeakOut.Models;

namespace SpeakOut.Intents.AMAZON
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
            return @"SpeakOut lets you send quick messages to friends and family. Ask SpeakOut for a friend code and exchange it with your friend, the ask SpeakOut to befriend them with their code to get set up.
            Then you can ask SpeakOut to tell your friend anything you want. Then you can check for new messages every so often to keep in touch. Lastly if you have lots of messages you don't want, 
            or have friends you no longer want to remeber, you can ask SpeakOut to forget them.";
        }
    }
}
