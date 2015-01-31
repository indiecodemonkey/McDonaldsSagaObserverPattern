using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.Events
{
    public class OrderReady : IEvent
    {
        public Guid OrderId { get; set; }
    }
}
