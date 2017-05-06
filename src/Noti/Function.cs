using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
        private readonly IServiceProvider provider;

        public Functions()
        {
            IServiceCollection services = new ServiceCollection();

            Configure(services);

            services.AddScoped<Context>();

            IEnumerable<Type> intents = this.GetType().GetTypeInfo().Assembly.GetTypes().Where(t => t.GetTypeInfo().BaseType == typeof(IntentBase)).ToList();

            Console.WriteLine("=== Utterances =========================");
            foreach ( Type intentType in intents )
            {
                services.AddScoped(intentType);
                printUtterances(intentType);
            }
            Console.WriteLine("========================================");

            provider = services.BuildServiceProvider();

            Console.WriteLine("=== Skill Definition ===================");

            Console.WriteLine(JsonConvert.SerializeObject(new { intents = intents.Select(it => new {
                name = it.Name.Replace("Intent", ""),
                samples = it.GetTypeInfo().GetCustomAttributes<UtteranceAttribute>().Select(u => u.Utterance),
                slots = it.GetTypeInfo().GetMethod("Invoke").GetParameters().Select(p => new { name = p.Name, type = p.Name})
            }),
                types = intents.SelectMany(i => i.GetMethod("Invoke").GetParameters()).Select(p => new { name = p.Name, values = new string[]{}}).GroupBy(x => x.name).Select(g => g.First())
            }, Formatting.Indented));
            
            Console.WriteLine("========================================");
        }

        private void printUtterances(Type intentType)
        {
            string intentName = intentType.Name.Replace("Intent", "");
            foreach ( UtteranceAttribute utterance in intentType.GetTypeInfo().GetCustomAttributes<UtteranceAttribute>() )
            {
                Console.WriteLine($"{intentName} {utterance.Utterance}");
            }
        }

        private void Configure(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "Redis:Uri", "localhost:6379" }
                })
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            
            var manager = new RedisManagerPool(config["Redis:Uri"]);

            services.AddScoped<IRedisClient>(x => manager.GetClient());
        }

        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Response response;
            IOutputSpeech innerResponse = null;

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                // default launch request, let's just let them know what you can do
                Console.WriteLine($"Default LaunchRequest made");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to number functions.  You can ask us to add numbers!";
            }

            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                // intent request, process the intent
                Console.WriteLine($"Intent Requested {input.Request.Intent.Name}");

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

                diScope.GetService<Context>().UserId = session.User.UserId;

                Console.WriteLine(JsonConvert.SerializeObject(new {
                    Message = "Invoking intent",
                    Intent = intent,
                    Slots = slots,
                    Session = session
                }));

                string intentTypeName = "Noti.Intents." + intent + "Intent";

                Type intentType = Type.GetType(intentTypeName);

                Object intentInstance = diScope.ServiceProvider.GetService(intentType);

                var intentMethodInfo = intentType.GetMethod("Invoke");

                object[] parameters = intentMethodInfo.GetParameters().Select(p => slots[p.Name].Value).ToArray();

                return (string) intentMethodInfo.Invoke(intentInstance, parameters);
            }
        }
    }
}