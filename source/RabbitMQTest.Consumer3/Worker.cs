using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Shared;
using RabbitMQTest.Shared.Extensions;
using RabbitMQTest.Shared.Models;
using RabbitMQTest.Shared.Services;
using System.Text;

namespace RabbitMQTest.Consumer3
{
    
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMQService rabbitService;
        private readonly IConnection connection;
        private readonly IModel channel;

        public Worker(ILogger<Worker> logger, IRabbitMQService rabbitService)
        {
            _logger = logger;
            this.rabbitService = rabbitService;


            var factory = new ConnectionFactory { HostName = "host.docker.internal", };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Extension methods
            channel.DeclareDeadLetterQueue();
            channel.DeclareQueue(Queues.Message);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            Console.WriteLine("Failed messages queue:");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(message);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(queue: Queues.DeadLetter, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}