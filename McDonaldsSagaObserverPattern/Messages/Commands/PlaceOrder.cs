﻿using System;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Messages.Commands
{
    public class PlaceOrder
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Shake Shake { get; set; }
        public Coffee Coffee { get; set; }
        public Drinks Drinks { get; set; }
        public Sandwich Sandwich { get; set; }
        public Fries Fries { get; set; }
    }
}