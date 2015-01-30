using System;

namespace McDonaldsSagaObserverPattern.Messages.Events
{
    public class FriesCompleted
    {
        public Guid OrderId { get; set; }
    }
}
