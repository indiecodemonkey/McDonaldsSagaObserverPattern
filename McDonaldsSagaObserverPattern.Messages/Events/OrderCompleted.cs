using System;

namespace McDonaldsSagaObserverPattern.Messages.Events
{
    public class OrderCompleted
    {
        public Guid OrderId { get; set; }
    }
}
