using System;

namespace McDonaldsSagaObserverPattern.Messages.InternalMessages
{
    public class ShakeCompleted
    {
        public Guid OrderId { get; set; }
    }
}
