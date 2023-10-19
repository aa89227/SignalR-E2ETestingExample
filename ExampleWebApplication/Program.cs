using ExampleWebApplication.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Services.AddLogging(builder =>
{
    builder.AddSimpleConsole(c =>
    {
        c.SingleLine = true;
        c.TimestampFormat = "[HH:mm:ss:fff] ";
    });
    builder.SetMinimumLevel(LogLevel.Trace);
});
var app = builder.Build();
app.UseResponseCompression();
app.MapHub<ExampleHub>("/examplehub");

app.Run();