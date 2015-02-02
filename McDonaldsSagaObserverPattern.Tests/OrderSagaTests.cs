using System;
using McDonaldsSagaObserverPattern.Messages;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using McDonaldsSagaObserverPattern.SagaEndpoint.Handlers;
using NServiceBus.Testing;
using NUnit.Framework;

namespace McDonaldsSagaObserverPattern.Tests
{
    public static class OrderSagaTests
    {
        [TestFixture]
        public class WhenPlacingAnOrderWithFriesAndAShake
        {
            private Saga<OrderSaga> sut;
            private PlaceOrder placeOrder;

            [SetUp]
            public void Given()
            {
                Test.Initialize();
                sut = Test.Saga<OrderSaga>();
                placeOrder = new PlaceOrder { OrderId = Guid.NewGuid(), Fries = new Fries(), Shake = new Shake() };
            }

            [Test]
            public void SendsMakeFries()
            {
                sut.ExpectSend<MakeFries>(cmd => cmd.OrderId == placeOrder.OrderId)
                    .When(saga => saga.Handle(placeOrder));
            }

            [Test]
            public void SendsMakeShake()
            {
                sut.ExpectSend<MakeShake>(cmd => cmd.OrderId == placeOrder.OrderId)
                    .When(saga => saga.Handle(placeOrder));
            }
        }

        [TestFixture]
        public class WhenHandlingFriesCompletedAndShakeIsNotDone
        {
            private Saga<OrderSaga> sut;
            private FriesCompleted friesCompleted;
            private PlaceOrder placeOrder;
            private Guid orderId;

            [SetUp]
            public void Given()
            {
                Test.Initialize();
                sut = Test.Saga<OrderSaga>();
                orderId = Guid.NewGuid();
                placeOrder = new PlaceOrder { OrderId = orderId, Fries = new Fries(), Shake = new Shake() };
                friesCompleted = new FriesCompleted { OrderId = orderId };
            }

            [Test]
            public void DoesNotPublishOrderCompleted()
            {
                sut.ExpectNotPublish<OrderCompleted>(cmd => cmd.OrderId == orderId)
                    .When(saga => saga.Handle(placeOrder))
                    .When(saga => saga.Handle(friesCompleted));
            }

            [Test]
            public void DoesNotMarkSagaComplete()
            {
                sut.AssertSagaCompletionIs(false)
                    .When(saga => saga.Handle(placeOrder))
                    .When(saga => saga.Handle(friesCompleted));
            }
        }

        [TestFixture]
        public class WhenHandlingShakeCompletedAndFriesAreNotDone
        {
            private Saga<OrderSaga> sut;
            private ShakeCompleted shakeCompleted;
            private PlaceOrder placeOrder;
            private Guid orderId;

            [SetUp]
            public void Given()
            {
                Test.Initialize();
                sut = Test.Saga<OrderSaga>();
                orderId = Guid.NewGuid();
                placeOrder = new PlaceOrder { OrderId = orderId, Fries = new Fries(), Shake = new Shake() };
                shakeCompleted = new ShakeCompleted { OrderId = orderId };
            }

            [Test]
            public void DoesNotPublishOrderCompleted()
            {
                sut.ExpectNotPublish<OrderCompleted>(cmd => cmd.OrderId == orderId)
                    .When(saga => saga.Handle(placeOrder))
                    .When(saga => saga.Handle(shakeCompleted));
            }

            [Test]
            public void DoesNotMarkSagaComplete()
            {
                sut.AssertSagaCompletionIs(false)
                    .When(saga => saga.Handle(placeOrder))
                    .When(saga => saga.Handle(shakeCompleted));
            }
        }

        [TestFixture]
        public class WhenBothFriesAndShakeAreDone
        {
            private Saga<OrderSaga> sut;
            private Guid orderId;
            private PlaceOrder placeOrder;
            private ShakeCompleted shakeCompleted;
            private FriesCompleted friesCompleted;

            [SetUp]
            public void Given()
            {
                Test.Initialize();
                sut = Test.Saga<OrderSaga>();
                orderId = Guid.NewGuid();
                placeOrder = new PlaceOrder { OrderId = orderId, Fries = new Fries(), Shake = new Shake() };
                shakeCompleted = new ShakeCompleted { OrderId = orderId };
                friesCompleted = new FriesCompleted { OrderId = orderId };
            }

            [Test]
            public void PublishesOrderCompleted()
            {
                sut
                    .When(saga => saga.Handle(placeOrder))
                    .When(saga => saga.Handle(shakeCompleted))
                    .ExpectPublish<OrderCompleted>(cmd => cmd.OrderId == orderId)
                    .When(saga => saga.Handle(friesCompleted));
            }

            [Test]
            public void MarksSagaComplete()
            {
                sut
                   .When(saga => saga.Handle(placeOrder))
                   .When(saga => saga.Handle(shakeCompleted))
                   .When(saga => saga.Handle(friesCompleted))
                   .AssertSagaCompletionIs(true);
            }
        }
    }
}
