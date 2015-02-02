using System;
using System.Collections.Generic;
using System.Linq;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

namespace McDonaldsSagaObserverPattern.SagaEndpoint.Handlers
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

            if (message.Fries != null)
            {
                AddMenuItemToOrderList("Fries");
                Bus.Send(new MakeFries { OrderId = message.OrderId, Fries = message.Fries });
            }

            if (message.Shake != null)
            {
                AddMenuItemToOrderList("Shake");
                Bus.Send(new MakeShake { OrderId = message.OrderId, Shake = message.Shake });
            }

            Log.Warn("order sent to all pertinenet stations.");
        }

        public void Handle(FriesCompleted message)
        {
            Log.Warn("handling FriesCompleted");
            UpdateMenuItemInOrderListToTrue("Fries");
        }

        public void Handle(ShakeCompleted message)
        {
            Log.Warn("handling ShakeCompleted");
            UpdateMenuItemInOrderListToTrue("Shake");
        }

        private void AddMenuItemToOrderList(string menuItem)
        {
            Data.OrderList.Add(menuItem);
        }

        private void UpdateMenuItemInOrderListToTrue(string menuItem)
        {
            Log.Warn(string.Format("updating menu item in order list for {0}", menuItem));
            Data.OrderList.Remove(menuItem);
            if (SagaIsDone())
                PublishOrderFinishedAndMarkSagaAsComplete();
        }

        private bool SagaIsDone()
        {
            return !Data.OrderList.Any();
        }

        private void PublishOrderFinishedAndMarkSagaAsComplete()
        {
            Log.Warn("publishing OrderCompleted");
            Bus.Publish(new OrderCompleted { OrderId = Data.OrderId });
            Log.Warn("marking Saga as complete");
            MarkAsComplete();
        }
        
        public class SagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }

            [Unique]
            public Guid OrderId { get; set; }
            public List<string> OrderList { get; set; }
            
            public SagaData()
            {
                OrderList = new List<string>();
            }
        }
    }
}
