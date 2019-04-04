using Common.Messages;
using Rebus.Bus;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Commands
{
    public class PublishCommand : IUserCommand
    {
        static readonly ILogger Logger = Log.ForContext<PublishCommand>();
        private readonly IBus bus;

        public PublishCommand(IBus bus)
        {
            this.bus = bus;
        }
        public string Description => "Publish some Subscribed messages";

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Enter the content of the messages to send:");
            var content = Console.ReadLine();
            var message = new SubscribedMessage { Content = content };

            Console.Write("How many?");
            int max;
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out max)) { }
            Console.WriteLine();

            foreach(var i in Enumerable.Range(0,max))
            {
                message.Id = i;
                await bus.Publish(message);
                Logger.Debug("Sent {@message}", message);
            }
        }
    }
}
