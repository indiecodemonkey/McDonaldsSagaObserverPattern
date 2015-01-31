using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class MakeShake : ICommand
    {
        public Guid OrderId { get; set; }
        public Shake Shake { get; set; }
    }
}