using RabbitMQ.Client;
using System.Runtime.CompilerServices;

namespace RabbitMQTest.Shared.Extensions
{
    public static class ChannelHelpers
    {

        public static string DeclareDeadLetterQueue(this IModel channel, string customDeadLetterQueue = "")
        {
            var deadLetterQueue = (String.IsNullOrWhiteSpace(customDeadLetterQueue))? Queues.DeadLetter : customDeadLetterQueue;

            channel.ExchangeDeclare(exchange: Exchanges.DeadLetterExchange, type: "fanout", durable: true, autoDelete: false);
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


            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            return deadLetterQueue;
        }



        public static IModel DeclareQueue(this IModel channel, string queueName, string deadLetterQueue = "")
        {

            string RetryQueue = (String.IsNullOrWhiteSpace(deadLetterQueue))? Queues.DeadLetter : deadLetterQueue;


            channel.ExchangeDeclare(exchange: Exchanges.MyApplicationExchange, type: "direct", durable: true, autoDelete: false);
            channel.QueueDeclare
            (
                queue: queueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false,
                new Dictionary<string, object> {
                        {"x-dead-letter-exchange", Exchanges.DeadLetterExchange},
                        {"x-dead-letter-routing-key", RetryQueue}
                }
            );
            channel.QueueBind(
                queue: queueName, 
                exchange:  Exchanges.MyApplicationExchange, 
                routingKey: string.Empty, 
                arguments: null);

            

            return channel;
        }
    }
}
