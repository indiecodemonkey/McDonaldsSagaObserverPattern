using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class MakeFries : ICommand
    {
        public Guid OrderId { get; set; }
        public Fries Fries { get; set; }
    }
}