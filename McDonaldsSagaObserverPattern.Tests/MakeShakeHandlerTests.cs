using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace McDonaldsSagaObserverPattern.Tests
{
    public static class MakeShakeHandlerTests
    {
        [TestFixture]
        public class WhenHandlingMakeShake
        {
            private Handler<MakeShakeHandler> sut;
            private MakeShake makeShake;

            [SetUp]
            public void Given()
            {
                //argh, we need to do this until v5 comes out? horrible: Check NServiceBusSetup in IXR 3 to see how this can be handled betteer. And why did I not run into this with Saga testing?
                //https://github.com/Particular/NServiceBus/issues/443
                MessageConventionExtensions.IsCommandTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("Commands") && !t.Namespace.StartsWith("NServiceBus");
                //MessageConventionExtensions.IsEventTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("Events") && !t.Namespace.StartsWith("NServiceBus");
                MessageConventionExtensions.IsMessageTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("InternalMessages") && !t.Namespace.StartsWith("NServiceBus");
                Test.Initialize();
                sut = Test.Handler(bus => new MakeShakeHandler(bus));
                makeShake = new MakeShake { OrderId = Guid.NewGuid() };
            }

            [Test]
            public void RepliesShakeCompleted()
            {
                sut.ExpectReply<ShakeCompleted>(m => m.OrderId == makeShake.OrderId)
                    .OnMessage(makeShake);
            }
        }
    }
}
