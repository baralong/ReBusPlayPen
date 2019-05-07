using Common.IO;
using Common.Messages;
using Rebus.Bus;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api.Commands
{
    class SagaCommand : IUserCommand
    {
        static readonly ILogger Logger = Log.ForContext<SagaCommand>();
        private readonly IBus bus;
        private readonly IConsole console;

        public SagaCommand(IBus bus, IConsole console)
        {
            this.bus = bus;
            this.console = console;
        }

        public string Description => "Start and interact with a saga";

        public async Task ExecuteAsync()
        {
            console.WriteLine("Enter the key for your saga:");
            var sagaKey = console.ReadLine();

            // Start
            await bus.Subscribe<SagaStateMessage>();
            var startMsg = new SagaStartMessage { Key = sagaKey };
            Logger.Debug("Sending {@StartMsg}", startMsg);
            await bus.Publish(startMsg);

            // Process
            console.WriteLine("Press a number key to increment the saga. Anything else to end.\n>");
            while (int.TryParse(console.ReadKey().KeyChar.ToString(), out var amount))
            {
                var incMsg = new SagaIncrementMessage { Key = sagaKey, Amount = amount };
                Logger.Debug("Sending {@IncMsg}", incMsg);
                await bus.Publish(incMsg);
                await Task.Delay(TimeSpan.FromMilliseconds(300));
                //while(console.KeyAvailable) console.ReadKey();
                console.Write("\n>");
            }

            // End
            console.WriteLine($"Ending Saga with key {sagaKey}");
            var endMsg = new SagaEndMessage { Key = sagaKey };
            Logger.Debug("Sending {@EndMsg}", endMsg);
            await bus.Publish(endMsg);
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            await bus.Unsubscribe<SagaStateMessage>();
        }
    }
}
