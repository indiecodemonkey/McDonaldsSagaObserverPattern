using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.Events;
using NServiceBus;

namespace McDonaldsSagaObserverPattern.Handlers
{
    public class MakeShakeHandler : IHandleMessages<MakeShake>
    {
        private readonly IBus bus;

        public MakeShakeHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MakeShake message)
        {
            bus.Reply(new ShakeCompleted { OrderId = message.OrderId });
        }
    }
}
