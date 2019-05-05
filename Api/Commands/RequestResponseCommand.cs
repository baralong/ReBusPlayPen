using Common.IO;
using Common.Messages;
using Rebus;
using Rebus.Bus;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api.Commands
{
    public class RequestResponseCommand : IUserCommand
    {
        static readonly ILogger Logger = Log.ForContext<RequestResponseCommand>();
        private readonly IBus bus;
        private readonly IConsole console;

        public RequestResponseCommand(IBus bus, IConsole console)
        {
            this.bus = bus;
            this.console = console;
        }
        public string Description => "Request a response";

        public async Task ExecuteAsync()
        {
            console.WriteLine("Enter the content of the message to send:");
            var content = console.ReadLine();
            var request = new RequestMessage { Content = content };
            Logger.Debug("Sending {@request}", request);
            var response = await bus.SendRequest<ResponseMessage>(request);
            Logger.Debug("Got {@response}", response);
        }
    }
}
