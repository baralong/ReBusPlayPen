using System.Threading.Tasks;
using Common.Messages;
using Rebus.Handlers;
using Serilog;

namespace Api
{
    internal class LogSagaState : IHandleMessages<SagaStateMessage>
    {
        static readonly ILogger Logger = Log.ForContext<LogSagaState>();
        public Task Handle(SagaStateMessage message)
        {
            Logger.Debug("Got {@SagaState}", message);
            return Task.CompletedTask;
        }
    }
}