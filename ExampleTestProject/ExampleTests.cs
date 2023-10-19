using ExampleWebApplication.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Concurrent;

namespace ExampleTestProject;

[TestClass]
public class ExampleTests
{
    private TestServer server = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        server = new TestServer();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        server.Dispose();
    }

    [TestMethod]
    public async Task BroadcastTestAsync()
    {
        // Arrange
        var clientA = server.CreateHubConnection("A");
        var clientB = server.CreateHubConnection("B");

        BlockingCollection<string> messagesA = new();
        BlockingCollection<string> messagesB = new();

        clientA.On<string>("Broadcast", message => messagesA.Add(message));
        clientB.On<string>("Broadcast", message => messagesB.Add(message));

        await clientA.StartAsync();
        await clientB.StartAsync();

        // Act
        await clientA.SendAsync("Broadcast", "Hello, World!");

        // Assert
        CancellationTokenSource cancellationTokenSource = new(20000);

        var messageA = messagesA.Take(cancellationTokenSource.Token);
        Assert.AreEqual("Hello, World!", messageA);
        var messageB = messagesB.Take(cancellationTokenSource.Token);
        Assert.AreEqual("Hello, World!", messageB);
    }

    [TestMethod]
    public async Task SendWithObjectTestAsync()
    {
        // Arrange
        var clientA = server.CreateHubConnection("A");
        var clientB = server.CreateHubConnection("B");

        BlockingCollection<Data> messagesA = new();
        BlockingCollection<Data> messagesB = new();

        clientA.On<Data>("SendObject", message => messagesA.Add(message));
        clientB.On<Data>("SendObject", message => messagesB.Add(message));

        await clientA.StartAsync();
        await clientB.StartAsync();

        // Act
        await clientA.SendAsync("SendWithObject", "Hello, World!");

        // Assert
        CancellationTokenSource cancellationTokenSource = new(20000);
        var messageA = messagesA.Take(cancellationTokenSource.Token);
        Assert.AreEqual(new Data("1", "Hello, World!"), messageA);
        var messageB = messagesB.Take(cancellationTokenSource.Token);
        Assert.AreEqual(new Data("1", "Hello, World!"), messageB);
    }

    [TestMethod]
    public async Task SendWithCollectionTestAsync()
    {
        // Arrange
        var clientA = server.CreateHubConnection("A");
        var clientB = server.CreateHubConnection("B");

        BlockingCollection<string[]> messagesA = new();
        BlockingCollection<string[]> messagesB = new();

        clientA.On<string[]>("SendCollection", message => messagesA.Add(message));
        clientB.On<string[]>("SendCollection", message => messagesB.Add(message));

        await clientA.StartAsync();
        await clientB.StartAsync();

        // Act
        await clientA.SendAsync("SendWithCollection", "Hello, World!", "Hello, World2!");
        // Assert

        CancellationTokenSource cancellationTokenSource = new(20000);
        var messageA = messagesA.Take(cancellationTokenSource.Token);
        CollectionAssert.AreEqual(new[] { "Hello, World!", "Hello, World2!" }, messageA);
        var messageB = messagesB.Take(cancellationTokenSource.Token);
        CollectionAssert.AreEqual(new[] { "Hello, World!", "Hello, World2!" }, messageB);
    }
}