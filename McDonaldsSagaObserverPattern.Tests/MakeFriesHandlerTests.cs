using System;
using McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
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
