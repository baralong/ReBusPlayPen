using System;
using System.Threading.Tasks;
using Common.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Serilog;

namespace Processor.Handlers
{
    public class RequestHandler : IHandleMessages<RequestMessage>
    {
        static readonly ILogger Logger = Log.ForContext<RequestHandler>();
        private readonly IBus _bus;

        public RequestHandler(IBus bus)
        {
            _bus = bus;
        }
        public Task Handle(RequestMessage request)
        {
            Logger.Debug("RequestHandler got {@request}", request);
            var response = new ResponseMessage
            {
                Content = DateTime.Now.ToString("s"),
                Echo = request.Content,
            };
            return _bus.Reply(response);
        }
    }
}
