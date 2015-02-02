using System;

namespace McDonaldsSagaObserverPattern.Messages.InternalMessages
{
    public class FriesCompleted
    {
        public Guid OrderId { get; set; }
    }
}
