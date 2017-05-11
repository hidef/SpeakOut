
using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using NSubstitute;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Requests.RequestTypes;
using Slight.Alexa.Framework.Models.Responses;
using Xunit;

namespace Noti.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void GetCode()
        {
            Functions f = new Functions();
            ILambdaContext ctx = Substitute.For<ILambdaContext>();
            var request = new SkillRequest
            {
                Request = new RequestBundle
                {
                    Type = "IntentRequest",
                    Intent = new Intent
                    {
                        Name = "GetCode",
                        Slots = new Dictionary<string, Slot> {
                        }
                    }
                },
                Session = new Session {
                    User = new User {
                        UserId = "jill"
                    }
                }
            };
            var response = f.FunctionHandler(request, null);

            Console.WriteLine(((PlainTextOutputSpeech) response.Response.OutputSpeech).Text);
        }
        
        [Fact]
        public void Befriend()
        {
            Functions f = new Functions();
            ILambdaContext ctx = Substitute.For<ILambdaContext>();
            var request = new SkillRequest
            {
                Request = new RequestBundle
                {
                    Type = "IntentRequest",
                    Intent = new Intent
                    {
                        Name = "Befriend",
                        Slots = new Dictionary<string, Slot> {
                            {"name", new Slot { Value = "jack"}},
                            {"code", new Slot { Value = "499"}}
                        }
                    }
                },
                Session = new Session {
                    User = new User {
                        UserId = "jill"
                    }
                }
            };
            var response = f.FunctionHandler(request, null);


            Console.WriteLine(((PlainTextOutputSpeech) response.Response.OutputSpeech).Text);

        }
        
        [Fact]
        public void Tell()
        {
            Functions f = new Functions();
            ILambdaContext ctx = Substitute.For<ILambdaContext>();
            var request = new SkillRequest
            {
                Request = new RequestBundle
                {
                    Type = "IntentRequest",
                    Intent = new Intent
                    {
                        Name = "Tell",
                        Slots = new Dictionary<string, Slot> {
                            {"recipient", new Slot { Value = "jack"}},
                            {"message", new Slot { Value = "don't fall down"}}
                        }
                    }
                },
                Session = new Session {
                    User = new User {
                        UserId = "jill"
                    }
                }
            };
            var response = f.FunctionHandler(request, null);

            Console.WriteLine(((PlainTextOutputSpeech) response.Response.OutputSpeech).Text);
        }

        
        [Fact]
        public void Check()
        {
            Functions f = new Functions();
            ILambdaContext ctx = Substitute.For<ILambdaContext>();
            var request = new SkillRequest
            {
                Request = new RequestBundle
                {
                    Type = "IntentRequest",
                    Intent = new Intent
                    {
                        Name = "Check",
                        Slots = new Dictionary<string, Slot> {
                        }
                    }
                },
                Session = new Session {
                    User = new User {
                        UserId = "jill"
                    }
                }
            };
            var response = f.FunctionHandler(request, null);

            Console.WriteLine(((PlainTextOutputSpeech) response.Response.OutputSpeech).Text);
        }
    }
}
