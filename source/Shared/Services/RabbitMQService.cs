using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQTest.Shared.Extensions;
using RabbitMQTest.Shared.Models;
using System.Text;

namespace RabbitMQTest.Shared.Services
{
    public interface IRabbitMQService
    {
        public void SendMessage(Message message);
        public void SendMessage(List<Message> message);
    }


    public class RabbitMQService : IRabbitMQService
    {
        private readonly string RabbitAddress;
        private readonly IConnection Connection;

        public RabbitMQService(IConfiguration configuration)
        {
            RabbitAddress = configuration.GetValue<string>("RabbitAddress");
            var factory = new ConnectionFactory { HostName = RabbitAddress };

            Connection = factory.CreateConnection();
        }


        public void SendMessage(Message message)
        {
            using var channel = Connection.CreateModel();
            channel.QueueDeclare(Queues.Message, exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: Queues.Message, body: body);
        }


        public void SendMessage(List<Message> messages)
        {
            using var channel = Connection.CreateModel();

            channel.DeclareDeadLetterQueue();
            channel.DeclareQueue(Queues.Message);

            var publishBatch = channel.CreateBasicPublishBatch();

            messages.ForEach(message =>
            {
                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var body = new ReadOnlyMemory<byte>(bytes);
                publishBatch.Add(exchange: "",routingKey: Queues.Message, mandatory: false, properties: null, body: body);
            });

            publishBatch.Publish();
        }
    }
}
