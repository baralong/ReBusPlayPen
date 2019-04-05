using Common.Messages;
using Rebus.Sagas;

namespace Processor.Saga
{
    public class SimpleData : SagaData
    {
        public string Key { get; set; }
        public int Amount { get; set; }
    }
}