﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Shared;
using RabbitMQTest.Shared.Extensions;
using RabbitMQTest.Shared.Models;
using System.Text;

var factory = new ConnectionFactory { HostName = "host.docker.internal", };

var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Extension methods
channel.DeclareDeadLetterQueue();
channel.DeclareQueue(Queues.Message);

Console.WriteLine("Consumer 2: Pauses 3 seconds on msg 15 and 16");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    try
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        var obj = JsonConvert.DeserializeObject<Message>(message);
        if (obj.Id == 10)
            throw new Exception("Let me go !!");

        if(obj.Id == 15 || obj.Id == 16)
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();

        Console.WriteLine(message);
        channel.BasicAck(eventArgs.DeliveryTag, false);
    }
    catch (Exception)
    {
        if (eventArgs.Redelivered)
            channel.BasicNack(eventArgs.DeliveryTag, false, false);
        else
            channel.BasicReject(eventArgs.DeliveryTag, true);
    }
};

channel.BasicConsume(queue: Queues.Message, autoAck: false, consumer: consumer);
Console.ReadKey();