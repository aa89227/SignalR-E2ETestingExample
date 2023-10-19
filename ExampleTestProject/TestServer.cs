﻿using ExampleWebApplication.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace ExampleTestProject;

internal class TestServer : WebApplicationFactory<Program>
{
    public HubConnection CreateHubConnection(string userId)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Server.BaseAddress}examplehub?userId={userId}", options =>
            {
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;
                options.HttpMessageHandlerFactory = _ => Server.CreateHandler();
            })
            .ConfigureLogging(logging =>
            {
                //logging.AddDebug();
                //logging.SetMinimumLevel(LogLevel.Trace);
            })
            .Build();
        return hubConnection;
    }

}