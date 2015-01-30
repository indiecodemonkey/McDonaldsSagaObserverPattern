using System;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class MakeFries
    {
        public Guid OrderId { get; set; }
        public Fries Fries { get; set; }
    }
}