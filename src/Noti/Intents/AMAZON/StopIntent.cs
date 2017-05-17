namespace Noti.Intents.AMAZON
{
    public class StopIntent : IntentBase
    {
        Context ctx;
        
        public StopIntent(Context ctx)
        {
            this.ctx = ctx;
        }

        public AlexaResponse Invoke()
        {
            return new AlexaResponse { 
                ResponseText = "",
                ShouldEndSession = true
            };
        }
    }
}
