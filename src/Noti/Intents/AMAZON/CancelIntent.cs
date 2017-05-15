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
