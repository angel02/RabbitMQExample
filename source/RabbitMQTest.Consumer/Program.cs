var app = DefaultApp.Create(builder =>
{

});


app.MapGet("/", () => "Say hello");


DefaultApp.Run(app);