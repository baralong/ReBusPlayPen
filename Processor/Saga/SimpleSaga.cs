using System.Threading.Tasks;
using Common.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Serilog;

namespace Processor.Saga
{
    class SimpleSaga : 
        Saga<SimpleData>, 
        IAmInitiatedBy<SagaStartMessage>,
        IHandleMessages<SagaIncrementMessage>,
        IHandleMessages<SagaEndMessage>
    {
        static readonly ILogger Logger = Log.ForContext<SimpleSaga>();
        private readonly IBus bus;

        public SimpleSaga(IBus bus)
        {
            this.bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<SimpleData> config)
        {
            config.Correlate<SagaStartMessage>(msg => msg.Key, data => data.Key);
            config.Correlate<SagaIncrementMessage>(msg => msg.Key, data => data.Key);
            config.Correlate<SagaEndMessage>(msg => msg.Key, data => data.Key);
        }

        public async Task Handle(SagaStartMessage message)
        {
            Logger.Debug("Saga started {@data}", Data);
            await PublishState(message.GetType().Name.Replace("Messsage", ""));
        }

        public async Task Handle(SagaIncrementMessage message)
        {
            Data.Amount += message.Amount;
            Logger.Debug("Saga Incremented {@data}", Data);
            await PublishState(message.GetType().Name.Replace("Messsage", ""));
        }

        public async Task Handle(SagaEndMessage message)
        {
            Logger.Debug("Saga ended {@data}", Data);
            await PublishState(message.GetType().Name.Replace("Messsage", ""));
            MarkAsComplete();
        }

        private Task PublishState(string handler)
        {
            return bus.Publish(new SagaStateMessage
            {
                Handler = handler,
                Key = Data.Key,
                Amount = Data.Amount,
            });
        }
    }
}
