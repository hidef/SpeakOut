using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using Noti;
using NSubstitute;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Requests.RequestTypes;

namespace Noti.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void TetGetMethod()
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
                            {"Recipient", new Slot { Value = "jack"}},
                            {"Message", new Slot { Value = "don't fall down"}}
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

        }
    }
}
