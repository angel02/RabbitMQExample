using RabbitMQTest.Consumer3;
using RabbitMQTest.Shared.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitMQService, RabbitMQService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
