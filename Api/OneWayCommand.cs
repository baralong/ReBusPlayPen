using Common.Messages;
using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace Api
{
    class OneWayCommand : IUserCommand
    {
        private readonly IBus bus;

        public OneWayCommand(IBus bus)
        {
            this.bus = bus;
        }
        public string Description => "Send a one-way message";

        public char Key => 'a';

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Enter the content of the message to send:");
            var content = Console.ReadLine();
            await bus.Send(new OneWayMessage { Content = content });
        }
    }
}
