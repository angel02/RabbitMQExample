using RabbitMQ.Client;
using System.Runtime.CompilerServices;

namespace RabbitMQTest.Shared.Extensions
{
    public static class ChannelHelpers
    {

        public static string DeclareDeadLetterQueue(this IModel channel, string customDeadLetterQueue = "")
        {
            var deadLetterQueue = (String.IsNullOrWhiteSpace(customDeadLetterQueue))? Queues.DeadLetter : customDeadLetterQueue;

            channel.ExchangeDeclare(Exchanges.DeadLetterExchange, "fanout");
            channel.QueueDeclare
            (
                queue: deadLetterQueue,
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                arguments: new Dictionary<string, object> {
                        { "x-dead-letter-exchange", Exchanges.MyApplicationExchange },
                        //{ "x-message-ttl", 30000 },
                }
            );

            channel.QueueBind(
                queue: deadLetterQueue, 
                exchange: Exchanges.DeadLetterExchange,
                routingKey: string.Empty, 
                arguments: null
            );

            return deadLetterQueue;
        }



        public static IModel DeclareQueue(this IModel channel, string queueName, string deadLetterQueue = "")
        {

            string RetryQueue = (String.IsNullOrWhiteSpace(deadLetterQueue))? Queues.DeadLetter : deadLetterQueue;


            channel.ExchangeDeclare(Exchanges.MyApplicationExchange, "direct");
            channel.QueueDeclare
            (
                queueName, true, false, false,
                new Dictionary<string, object>
                {
                        {"x-dead-letter-exchange", Exchanges.DeadLetterExchange},
                        {"x-dead-letter-routing-key", RetryQueue}
                }
            );
            channel.QueueBind(queueName, Exchanges.MyApplicationExchange, string.Empty, null);

            

            return channel;
        }
    }
}
