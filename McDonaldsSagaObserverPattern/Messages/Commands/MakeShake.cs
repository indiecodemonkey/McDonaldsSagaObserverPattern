using System;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class MakeShake
    {
        public Guid OrderId { get; set; }
        public Shake Shake { get; set; }
    }
}