var app = DefaultApp.Create(builder =>
{
    
});


app.MapGet("/", () => "Hello World!");



DefaultApp.Run(app);