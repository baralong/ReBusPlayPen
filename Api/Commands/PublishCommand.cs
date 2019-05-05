using Common.IO;
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
        private readonly IConsole console;

        public PublishCommand(IBus bus, IConsole console)
        {
            this.bus = bus;
            this.console = console;
        }
        public string Description => "Publish some Subscribed messages";

        public async Task ExecuteAsync()
        {
            console.WriteLine("Enter the content of the messages to send:");
            var content = console.ReadLine();
            var message = new SubscribedMessage { Content = content };

            console.Write("How many?");
            int max;
            while (!int.TryParse(console.ReadKey().KeyChar.ToString(), out max)) { }
            console.WriteLine();

            foreach(var i in Enumerable.Range(0,max))
            {
                message.Id = i;
                await bus.Publish(message);
                Logger.Debug("Sent {@message}", message);
            }
        }
    }
}
