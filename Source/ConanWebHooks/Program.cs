using ConanWebHooks.Logic;
using ConanWebHooks.Models;
using ConanWebHooks.Services;
using Microsoft.Extensions.Options;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddTransient<IDataService, FileSystemService>();
    builder.Services.AddTransient<WebHookService>();
    builder.Services.AddTransient<GameLogHandler>();
    builder.Services.AddTransient<ChatHandler>();
    builder.Services.AddTransient<SpawnHandler>();
    builder.Services.AddSingleton<SettingsService>();

    builder.Services.AddEndpointsApiExplorer();
#if DEBUG
    builder.Services.AddSwaggerGen();
#endif

    builder.Host.UseSerilog((context, serviceProvider, configuration) =>
                            {
                                //configuration.ReadFrom.Configuration(context.Configuration);
                                //var data = serviceProvider.GetRequiredService<IOptions<DiscordData>>().Value;

                                //foreach (var hook in data.ServerHooks)
                                //{
                                //    var server = hook.Server.ToUpperInvariant();
                                //    if(hook.SeparateLog)
                                //    {
                                //        configuration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly($"StartsWith(@m, '[{server}]')")
                                //                                             .WriteTo.File($"Logs/{server}-log-.txt",
                                //                                               rollingInterval: RollingInterval.Day,
                                //                                               rollOnFileSizeLimit: true,
                                //                                               fileSizeLimitBytes: 10_485_760,
                                //                                               retainedFileCountLimit: 14));
                                //    }
                                //}
                            });

    var app = builder.Build();

#if DEBUG
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
#endif

    app.MapGet("/{server}/log", async Task (GameLogHandler service, [AsParameters]LogQueryModel logData) => await service.ReceiveData(logData));

    app.MapGet("/{server}/chat", async Task (ChatHandler service, [AsParameters]ChatQueryModel chatData) => await service.ReceiveData(chatData));
    
    app.MapGet("/{server}/spawn", async Task (SpawnHandler service, [AsParameters]SpawnQueryModel chatData) => await service.ReceiveData(chatData));

    Log.Information("Starting application.");
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