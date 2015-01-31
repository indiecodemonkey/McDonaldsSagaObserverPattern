using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsSagaObserverPattern.Messages;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace McDonaldsSagaObserverPattern
{
    public class OrderSaga : Saga<OrderSaga.SagaData>, 
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<FriesCompleted>, 
        IHandleMessages<ShakeCompleted>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderSaga));

        public void Handle(PlaceOrder message)
        {
            Log.Warn("order placed.");
            Data.OrderId = message.OrderId;

            if (message.Shake != null)
            {
                Data.OrderList.Add(typeof(Shake), false);
                Bus.Send(new MakeShake { OrderId = message.OrderId, Shake = message.Shake });
            }

            if (message.Fries != null)
            {
                Data.OrderList.Add(typeof(Fries), false);
                Bus.Send(new MakeFries { OrderId = message.OrderId, Fries = message.Fries });
            }
            Log.Warn("order sent to all stations.");
            //and so on for the rest of the menu items
        }

        public void Handle(FriesCompleted message)
        {
            Data.OrderList[typeof (Fries)] = true;
            if (SagaIsDone())
                PublishOrderFinishedAndMarkSagaAsComplete();
        }

        public void Handle(ShakeCompleted message)
        {
            Data.OrderList[typeof(Shake)] = true;
            if (SagaIsDone())
                PublishOrderFinishedAndMarkSagaAsComplete();
        }

        private void PublishOrderFinishedAndMarkSagaAsComplete()
        {
            Bus.Publish(new OrderReady { OrderId = Data.OrderId });
            MarkAsComplete();
        }

        private bool SagaIsDone()
        {
            if (Data.OrderList.Values.Any(x => false)) return false;
            return true;
            //return !Data.OrderList.Values.Any(x => false);
        }

        public class SagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }

            [Unique]
            public Guid OrderId { get; set; }
            //do I need to new this up in the message tha starts the saga?
            public Dictionary<Type, bool> OrderList { get; set; }
        }
    }
}
