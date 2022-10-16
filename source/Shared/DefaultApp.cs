using RabbitMQTest.Shared.Services;

public static class DefaultApp
{
    public static WebApplication Create(Action<WebApplicationBuilder>? webAppBuilder = null)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (webAppBuilder != null)
            webAppBuilder.Invoke(builder);

        return builder.Build();
    }

    public static void Run(WebApplication webApp)
    {
        if (webApp.Environment.IsDevelopment())
        {
            webApp.UseSwagger();
            webApp.UseSwaggerUI();
        }

        //webApp.UseHttpsRedirection();
        webApp.Run();
    }
}