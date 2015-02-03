using System;
using McDonaldsSagaObserverPattern.MenuStationEndpoint.Handlers;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
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
