using System.Threading.Tasks;
using Common.Messages;
using Rebus.Handlers;
using Serilog;

namespace Processor
{
    class OneWayHandler : IHandleMessages<OneWayMessage>
    {
        static readonly ILogger Logger = Log.ForContext<OneWayHandler>();
 
        public Task Handle(OneWayMessage message)
        {
            Logger.Debug("OneWayHandler got {@message}", message);
            return Task.CompletedTask;
        }
    }
}
