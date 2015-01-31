using System;
using McDonaldsSagaObserverPattern.Messages;
using McDonaldsSagaObserverPattern.Messages.Commands;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.ClassClient
{
    public class MessageSender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Press 'Enter' to place an Order.To exit, Ctrl + C");
            var placeOrder = new PlaceOrder { OrderId = Guid.NewGuid(), Fries = new Fries(), Shake = new Shake() };
            Bus.Send(placeOrder);
        }

        public void Stop()
        {
        }
    }
}
