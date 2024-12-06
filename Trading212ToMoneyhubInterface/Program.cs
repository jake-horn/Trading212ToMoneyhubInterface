using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Trading212ToMoneyhubInterface;
using Trading212ToMoneyhubInterface.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(@"C:\Temp\app.log", rollingInterval: RollingInterval.Month)
    .CreateLogger();

var services = new ServiceCollection();

services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

// Register the services
services.AddSingleton<IConfiguration>(configuration); 
services.AddSingleton<IApp, App>();
services.AddSingleton<ITrading212Service, Trading212Service>();
services.AddSingleton<IMoneyhubService, MoneyhubService>(); 

// Set up HttpClient
string httpClientName = configuration.GetRequiredSection("Trading212:HttpClientName").Value!;

services.AddHttpClient(httpClientName,client =>
    {
        client.BaseAddress = new Uri("https://live.trading212.com/api/v0/");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(configuration.GetRequiredSection("Trading212:ApiKey").Value!);
    });

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Resolve the services you need
var appStart = serviceProvider.GetRequiredService<IApp>(); 
await appStart.Run();

Log.CloseAndFlush();