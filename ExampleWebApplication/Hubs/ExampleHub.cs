using Microsoft.AspNetCore.SignalR;

namespace ExampleWebApplication.Hubs;

public class ExampleHub : Hub<IExampleHubResponses>
{
    private readonly ILogger<ExampleHub> _logger;
    public ExampleHub(ILogger<ExampleHub> logger)
    {
        _logger = logger;
    }

    public async Task Broadcast(string message)
    {
        _logger.LogTrace("【Server】 Recieve(Broadcast):  '{message}'", message);
        await Clients.All.Broadcast(message);
        _logger.LogTrace("【Server】 Send(Broadcast):  '{message}'", message);
    }

    public async Task SendWithObject(string message)
    {
        _logger.LogTrace("【Server】 Recieve(SendWithObject):  '{message}'", message);
        await Clients.All.SendObject(new("1", message));
        _logger.LogTrace("【Server】 Send(Broadcast):  '{message}'", new Data("1", message));
    }

    public async Task SendWithCollection(string message, string message2)
    {
        _logger.LogTrace("【Server】 Recieve(SendWithCollection):  '{message} {message2}'", message, message2);
        await Clients.All.SendCollection(new[] { message, message2 });
        _logger.LogTrace("【Server】 Send(Broadcast):  '{message}'", new[] { message, message2 });
    }
}