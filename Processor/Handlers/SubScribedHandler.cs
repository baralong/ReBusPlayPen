using System;
using System.Threading.Tasks;
using Common.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Serilog;

namespace Processor.Handlers
{
    public class SubscribeHandler : IHandleMessages<SubscribedMessage>
    {
        static readonly ILogger Logger = Log.ForContext<SubscribeHandler>();
        private readonly IBus _bus;

        public SubscribeHandler(IBus bus)
        {
            _bus = bus;
        }
        public Task Handle(SubscribedMessage subscribed)
        {
            Logger.Debug("SubscribeHandler got {@subscribed}", subscribed);
            return Task.CompletedTask;
        }
    }
}
