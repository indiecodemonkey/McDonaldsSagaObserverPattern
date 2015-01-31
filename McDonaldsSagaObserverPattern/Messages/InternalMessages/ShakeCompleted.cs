using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.InternalMessages
{
    public class ShakeCompleted : IMessage
    {
        public Guid OrderId { get; set; }
    }
}
