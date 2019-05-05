using Api.Commands;
using Common.IO;
using Common.Messages;
using NSubstitute;
using NUnit.Framework;
using Rebus.Bus;
using System.Threading.Tasks;

namespace Api.Tests
{
    [TestFixture]
    public class OneWayCommandTest
    {
        [Test]
        public async Task Executed_MessagePassedToBus()
        {
            var console = Substitute.For<IConsole>();
            console.ReadLine().Returns("Test Data");
            var bus = Substitute.For<IBus>();
            var command = new OneWayCommand(bus, console);
            await command.ExecuteAsync();
            await bus.Received().Send(Arg.Is<OneWayMessage>(msg => msg.Content == "Test Data"));
        }
    }
}
