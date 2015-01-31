using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.InternalMessages
{
    public class FriesCompleted : IMessage
    {
        public Guid OrderId { get; set; }
    }
}
