namespace RabbitMQTest.Shared
{
    public class Queues
    {
        public const string Message = "Messages";
        public const string DeadLetter = "FailedMessages";
    }

    public class Exchanges
    {
        public const string MyApplicationExchange = "my-application-exchange";
        public const string DeadLetterExchange = "dead-letter-exchange";
    }
}
