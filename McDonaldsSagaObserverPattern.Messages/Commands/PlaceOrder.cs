using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class PlaceOrder : ICommand
    {
        public Guid OrderId { get; set; }
        public Shake Shake { get; set; }
        public Fries Fries { get; set; }
        public Coffee Coffee { get; set; }
        public Drinks Drinks { get; set; }
        public Sandwich Sandwich { get; set; }
    }
}