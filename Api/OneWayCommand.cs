﻿using Common.Messages;
using Rebus.Bus;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api
{
    class OneWayCommand : IUserCommand
    {
        static readonly ILogger Logger = Log.ForContext<OneWayCommand>();
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
            OneWayMessage message = new OneWayMessage { Content = content };
            await bus.Send(message);
            Logger.Debug("Sent {@message}", message);
        }
    }
}
