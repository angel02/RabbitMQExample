using RabbitMQTest.Shared.Models;
using RabbitMQTest.Shared.Services;

var app = DefaultApp.Create(builder => { });


app.MapGet("/", (IRabbitMQService rabbitService) => {

    var messages = new List<Message>();

    for (int i = 1; i <= 30; i++) {
        messages.Add(new Message() { Id = i, Title = "Message " + i, Content = "Content " + i });
    }

    rabbitService.SendMessage(messages);

    return "It works !!";
});


app.MapPost("/", (Message message, IRabbitMQService rabbitService) => {
    rabbitService.SendMessage(message);
    return "Sent";
});



DefaultApp.Run(app);