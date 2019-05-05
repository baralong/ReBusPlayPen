using Common.IO;
using Common.Messages;
using Rebus.Bus;
using Serilog;
using System.Threading.Tasks;

namespace Api.Commands
{
    public class OneWayCommand : IUserCommand
    {
        static readonly ILogger Logger = Log.ForContext<OneWayCommand>();
        private readonly IBus bus;
        private readonly IConsole console;

        public OneWayCommand(IBus bus, IConsole console)
        {
            this.bus = bus;
            this.console = console;
        }
        public string Description => "Send a one-way message";

        public async Task ExecuteAsync()
        {
            console.WriteLine("Enter the content of the message to send:");
            var content = console.ReadLine();
            var message = new OneWayMessage { Content = content };
            await bus.Send(message);
            Logger.Debug("Sent {@message}", message);
        }
    }
}
