namespace Common.Messages
{
    public class SagaStateMessage
    {
        public string Handler { get; set; }
        public int Amount { get; set; }
        public string Key { get; set; }
    }

}
