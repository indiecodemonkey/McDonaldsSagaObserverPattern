using System.Threading;
using McDonaldsSagaObserverPattern.Messages.Commands;
using McDonaldsSagaObserverPattern.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace McDonaldsSagaObserverPattern.SagaEndpoint.Handlers
{
    public class MakeShakeHandler : IHandleMessages<MakeShake>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MakeShakeHandler));
        private readonly IBus bus;

        public MakeShakeHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MakeShake message)
        {
            Log.Warn("starting to make shake");
            Thread.Sleep(3000); //3 seconds
            Log.Warn("shake done");
            bus.Reply(new ShakeCompleted { OrderId = message.OrderId });
        }
    }
}
