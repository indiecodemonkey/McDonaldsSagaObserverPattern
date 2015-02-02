using System;
using McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace McDonaldsSagaObserverPattern.Tests
{
    public static class MakeFriesHandlerTests
    {
        [TestFixture]
        public class WhenHandlingMakeFries
        {
            private Handler<MakeFriesHandler> sut;
            private MakeFries makeFries;

            [SetUp]
            public void Given()
            {
                //argh, we need to do this until v5 comes out? horrible: Check NServiceBusSetup in IXR 3 to see how this can be handled betteer. And why did I not run into this with Saga testing?
                //https://github.com/Particular/NServiceBus/issues/443
                MessageConventionExtensions.IsCommandTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("Commands") && !t.Namespace.StartsWith("NServiceBus");
                //MessageConventionExtensions.IsEventTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("Events") && !t.Namespace.StartsWith("NServiceBus");
                MessageConventionExtensions.IsMessageTypeAction = t => t.Namespace != null && t.Namespace.EndsWith("InternalMessages") && !t.Namespace.StartsWith("NServiceBus");
                Test.Initialize();
                sut = Test.Handler(bus => new MakeFriesHandler(bus));
                makeFries = new MakeFries { OrderId = Guid.NewGuid() };
            }

            [Test]
            public void RepliesFriesCompleted()
            {
                sut.ExpectReply<FriesCompleted>(m => m.OrderId == makeFries.OrderId)
                    .OnMessage(makeFries);
            }
        }
    }
}
