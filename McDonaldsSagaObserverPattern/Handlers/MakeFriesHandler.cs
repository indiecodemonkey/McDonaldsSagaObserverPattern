using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Handlers
{
    public class MakeFriesHandler : IHandleMessages<MakeFries>
    {
        private readonly IBus bus;

        public MakeFriesHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MakeFries message)
        {
            bus.Reply(new FriesCompleted { OrderId = message.OrderId });
        }
    }
}
