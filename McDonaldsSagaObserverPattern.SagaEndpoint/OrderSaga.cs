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

namespace McDonaldsSagaObserverPattern.SagaEndpoint
{
    public class OrderSaga : Saga<OrderSaga.SagaData>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleMessages<FriesCompleted>,
        IHandleMessages<ShakeCompleted>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderSaga));

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<PlaceOrder>(m => m.OrderId).ToSaga(data => data.OrderId);
            ConfigureMapping<FriesCompleted>(m => m.OrderId).ToSaga(data => data.OrderId);
            ConfigureMapping<ShakeCompleted>(m => m.OrderId).ToSaga(data => data.OrderId);
        }

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
            Log.Warn("handling FriesCompleted");
            Data.OrderList[typeof(Fries)] = true;
            if (SagaIsDone())
                PublishOrderFinishedAndMarkSagaAsComplete();
        }

        public void Handle(ShakeCompleted message)
        {
            Log.Warn("handling ShakeCompleted");
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
            //foreach (var value in Data.OrderList.Values)
            //{
            //    if (value == false)
            //        return false;
            //}
            //return true;
            return Data.OrderList.Values.All(value => value != false);
        }

        public class SagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }

            [Unique]
            public Guid OrderId { get; set; }
            public Dictionary<Type, bool> OrderList { get; set; }

            public SagaData()
            {
                OrderList = new Dictionary<Type, bool>();
            }
        }
    }
}
