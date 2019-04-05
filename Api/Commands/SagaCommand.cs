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

        public SagaCommand(IBus bus)
        {
            this.bus = bus;
        }

        public string Description => "Start and interact with a saga";

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Enter the key for your saga:");
            var sagaKey = Console.ReadLine();

            // Start
            await bus.Subscribe<SagaStateMessage>();
            var startMsg = new SagaStartMessage { Key = sagaKey };
            Logger.Debug("Sending {@StartMsg}", startMsg);
            await bus.Publish(startMsg);

            // Process
            Console.WriteLine("Press a number key to increment the saga. Anything else to end.\n>");
            while (int.TryParse(Console.ReadKey().KeyChar.ToString(), out var amount))
            {
                var incMsg = new SagaIncrementMessage { Key = sagaKey, Amount = amount };
                Logger.Debug("Sending {@IncMsg}", incMsg);
                await bus.Publish(incMsg);
                await Task.Delay(TimeSpan.FromMilliseconds(300));
                while(Console.KeyAvailable) Console.ReadKey();
                Console.Write("\n>");
            }

            // End
            Console.WriteLine($"Ending Saga with key {sagaKey}");
            var endMsg = new SagaEndMessage { Key = sagaKey };
            Logger.Debug("Sending {@EndMsg}", endMsg);
            await bus.Publish(endMsg);
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            await bus.Unsubscribe<SagaStateMessage>();
        }
    }
}
