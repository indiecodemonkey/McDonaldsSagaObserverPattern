using System;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class PlaceOrder
    {
        public Guid OrderId { get; set; }
        public Shake Shake { get; set; }
        public Fries Fries { get; set; }
    }
}