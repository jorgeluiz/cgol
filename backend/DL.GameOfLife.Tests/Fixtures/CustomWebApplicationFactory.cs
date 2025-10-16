using DL.GameOfLife.Data.Context;
using DL.GameOfLife.Domain.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;

namespace DL.GameOfLife.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory for setting up an in-memory test server.
/// This class handles the bootstrapping of the API and overrides service registrations
/// to use test-specific dependencies, like an in-memory MongoDB server.
/// </summary>
/// <typeparam name="TProgram">The entry point of the application, typically the 'Program' class.</typeparam>
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private static MongoDbRunner? _mongoRunner;
    public IConfiguration Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Start the in-memory MongoDB server once for the test run.
        _mongoRunner = MongoDbRunner.Start();

        builder.ConfigureAppConfiguration(config =>
        {
            // Build a configuration to read appsettings and add our custom test settings.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    // Override database settings to point to the in-memory MongoDB instance.
                    ["DatabaseOptions:ConnectionString"] = _mongoRunner.ConnectionString,
                    ["DatabaseOptions:DatabaseName"] = $"GameOfLifeTestDb_{Guid.NewGuid()}",
                    
                    // Override application settings for predictable test outcomes.
                    ["GameOfLifeOptions:StatesIncrementLimit"] = "10" 
                })
                .Build();
            
            config.AddConfiguration(Configuration);
        });

        builder.ConfigureServices(services =>
        {
            // Find and remove the original IGameOfLifeContext registration.
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IGameOfLifeContext));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Add the GameOfLifeContext with options pointing to our in-memory MongoDB.
            services.AddSingleton<IGameOfLifeContext>(sp =>
            {
                var options = new DatabaseOptions
                {
                    ConnectionString = _mongoRunner.ConnectionString,
                    DatabaseName = Configuration.GetValue<string>("DatabaseOptions:DatabaseName")
                };
                // We don't need IOptions here since we are manually providing the configured object.
                return new GameOfLifeContext(Microsoft.Extensions.Options.Options.Create(options));
            });
        });
    }

    // Ensure the MongoDB runner is disposed of when the factory is disposed.
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _mongoRunner?.Dispose();
        }
    }
}
