// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.

namespace Noti
{
    public class AlexaResponse
    {
        public string ResponseText { get; set; }
        public bool ShouldEndSession { get; set; }

        public static implicit operator AlexaResponse(string value)
        {
            return new AlexaResponse {
                ResponseText = value
            };
        }
    }
}