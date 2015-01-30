using System;

namespace McDonaldsSagaObserverPattern.Messages.Events
{
    public class ShakeCompleted
    {
        public Guid OrderId { get; set; }
    }
}
