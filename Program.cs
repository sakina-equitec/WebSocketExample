using WebSocketExample;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var handler = new WebSocketHandler();
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await handler.HandleWebSocketAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.MapGet("/", () => "Hello World!");

app.Run();
