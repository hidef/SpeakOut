using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Noti.Intents;
using ServiceStack.Redis;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Responses;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.

namespace Noti
{
    public class Functions
    {
        private IServiceProvider provider;
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
            DefaultServiceProviderFactory fact = new DefaultServiceProviderFactory();
            IServiceCollection services = fact.CreateBuilder(new ServiceCollection());
            IServiceProvider provider = fact.CreateServiceProvider(services);

            Configure(services);
        }

        private void Configure(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            
            var manager = new RedisManagerPool(config["Redis:Uri"]);

            services.AddTransient<IRedisClient>(x => manager.GetClient());

            services.AddTransient<TellIntent>();
            services.AddTransient<CheckIntent>();
            services.AddTransient<DeleteIntent>();

        }

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Response response;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                // default launch request, let's just let them know what you can do
                log.LogLine($"Default LaunchRequest made");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to number functions.  You can ask us to add numbers!";
            }

            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                // intent request, process the intent
                log.LogLine($"Intent Requested {input.Request.Intent.Name}");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = invokeIntent(input.Request.Intent.Name, input.Request.Intent.Slots, input.Session);
            }

            response = new Response();
            response.ShouldEndSession = true;
            response.OutputSpeech = innerResponse;
            SkillResponse skillResponse = new SkillResponse();
            skillResponse.Response = response;
            skillResponse.Version = "1.0";

            return skillResponse;
        }

        private string invokeIntent(string intent, Dictionary<string, Slot> slots, Session session)
        {   
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

            using (var diScope = scopeFactory.CreateScope())
            {

                Console.WriteLine(JsonConvert.SerializeObject(new {
                    Message = "Invoking intent",
                    Intent = intent,
                    Slots = slots,
                    Session = session
                }));
                
                switch ( intent )
                {
                    case "Delete":
                        return diScope.GetService<DeleteIntent>().Invoke("Robert");
                    case "Check":
                        return diScope.GetService<CheckIntent>().Invoke("Robert");
                    case "Tell":
                        return diScope.GetService<TellIntent>().Invoke("Robert", slots["Recipient"].Value, slots["Message"].Value);
                    default:
                        return "Unknown intent";
                }
            }
        }
    }
}