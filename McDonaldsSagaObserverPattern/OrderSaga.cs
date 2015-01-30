using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsSagaObserverPattern.Messages;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using NServiceBus;
using NServiceBus.Saga;

namespace McDonaldsSagaObserverPattern
{
    public class OrderSaga : Saga<OrderSaga.SagaData>, 
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<FriesCompleted>, 
        IHandleMessages<ShakeCompleted>
    {
        public void Handle(PlaceOrder message)
        {
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
            //and so on for the rest of the menu items
        }

        public void Handle(FriesCompleted message)
        {
            Data.OrderList[typeof (Fries)] = true;
            if (SagaIsDone())
                MarkAsComplete();
        }

        public void Handle(ShakeCompleted message)
        {
            Data.OrderList[typeof(Shake)] = true;
            if (SagaIsDone())
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
            public Dictionary<Type, bool> OrderList { get; set; }
        }
    }
}
