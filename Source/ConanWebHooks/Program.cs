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

    if (builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddUserSecrets<DiscordData>(true);
    }

    builder.Services.AddWindowsService();
    builder.Services.AddHostedService<WindowsService>();
    builder.Services.Configure<DiscordData>(builder.Configuration.GetSection(DiscordData.SectionName));
    builder.Services.AddTransient<WebHookService>();
    builder.Services.AddTransient<DiscordService>();
    builder.Services.AddEndpointsApiExplorer();
#if DEBUG
    builder.Services.AddSwaggerGen();
#endif

    builder.Host.UseSerilog((context, serviceProvider, configuration) =>
                            {
                                configuration.ReadFrom.Configuration(context.Configuration);
                                var data = serviceProvider.GetRequiredService<IOptions<DiscordData>>().Value;

                                foreach (var hook in data.ServerHooks)
                                {
                                    var server = hook.Server.ToUpperInvariant();
                                    if(hook.SeparateLog)
                                    {
                                        configuration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly($"StartsWith(@m, '[{server}]')")
                                                                             .WriteTo.File($"Logs/{server}/log-.txt",
                                                                               rollingInterval: RollingInterval.Day,
                                                                               rollOnFileSizeLimit: true,
                                                                               fileSizeLimitBytes: 10_485_760,
                                                                               retainedFileCountLimit: 14));
                                    }
                                }
                            });

    var app = builder.Build();

#if DEBUG
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
#endif

    app.MapGet("/{server}/log", async Task (DiscordService service, [AsParameters]LogData logData) => await service.LogWebHook(logData));

    app.MapGet("/{server}/chat", async Task (DiscordService service, [AsParameters]ChatData chatData) => await service.ChatWebHook(chatData));

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