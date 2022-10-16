using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQTest.Shared.Extensions;
using RabbitMQTest.Shared.Models;
using System.Text;
using System.Threading.Channels;

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
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IBasicProperties properties;

        public RabbitMQService(IConfiguration configuration)
        {
            RabbitAddress = configuration.GetValue<string>("RabbitAddress");
            var factory = new ConnectionFactory { HostName = RabbitAddress };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.DeclareDeadLetterQueue();
            channel.DeclareQueue(Queues.Message);

            properties = channel.CreateBasicProperties();
            properties.Persistent = true;
        }


        public void SendMessage(Message message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: Queues.Message, basicProperties: properties, body: body);
        }


        public void SendMessage(List<Message> messages)
        {
            var publishBatch = channel.CreateBasicPublishBatch();


            messages.ForEach(message =>
            {
                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var body = new ReadOnlyMemory<byte>(bytes);
                publishBatch.Add(exchange: "",routingKey: Queues.Message, mandatory: false, properties: properties, body: body);
            });

            publishBatch.Publish();
        }
    }
}
