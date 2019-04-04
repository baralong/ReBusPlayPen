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
        static readonly ILogger Logger = Log.ForContext<OneWayCommand>();
        private readonly IBus bus;

        public RequestResponseCommand(IBus bus)
        {
            this.bus = bus;
        }
        public string Description => "Request a response";

        public char Key => 's';

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Enter the content of the message to send:");
            var content = Console.ReadLine();
            var request = new RequestMessage { Content = content };
            Logger.Debug("Sending {@request}", request);
            var response = await bus.SendRequest<ResponseMessage>(request);
            Logger.Debug("Got {@response}", response);
        }
    }
}
