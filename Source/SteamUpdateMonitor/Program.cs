using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SteamUpdateMonitor;
using SteamUpdateMonitor.Interfaces;
using SteamUpdateMonitor.Models;
using SteamUpdateMonitor.Services;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
	var builder = Host.CreateApplicationBuilder(args);

    builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetRequiredSection("Application"));
    builder.Services.Configure<DiscordSettings>(builder.Configuration.GetRequiredSection("Discord"));
    builder.Services.Configure<SteamSettings>(builder.Configuration.GetRequiredSection("Steam"));

    builder.Services.AddTransient<IFileSystem, PhysicalFileSystem>();
    builder.Services.AddTransient<ISteamApiService, SteamApiService>();
    builder.Services.AddTransient<IDiscordService, DiscordService>();
    builder.Services.AddHostedService<SteamMonitor>();
    
    builder.Services.AddSerilog((provider, config) =>
    {
        config.ReadFrom.Configuration(provider.GetRequiredService<IConfiguration>());
    });

    var app = builder.Build();
	app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Application is shutting down.");
    Log.CloseAndFlush();
}