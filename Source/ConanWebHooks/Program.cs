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
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<WebHookService>();
    builder.Services.AddTransient<DiscordService>();

    builder.Host.UseSerilog((context, serviceProvider, configuration) =>
                            {
                                var data = serviceProvider.GetRequiredService<IOptions<DiscordData>>().Value;
                                LoggerConfiguration common = null!;
                                configuration.WriteTo.Console()
                                             .WriteTo.Logger(lc =>
                                                             {
                                                                 lc.WriteTo.File($"Logs/log-.txt",
                                                                      rollingInterval: RollingInterval.Day,
                                                                      rollOnFileSizeLimit: true,
                                                                      fileSizeLimitBytes: 10_485_760,
                                                                      retainedFileCountLimit: 14);
                                                                 common = lc;
                                                             });

                                foreach (var hook in data.ServerHooks)
                                {
                                    var server = hook.Server.ToUpperInvariant();
                                    configuration.WriteTo.Logger(lc => lc
                                                                      .Filter
                                                                      .ByIncludingOnly($"StartsWith(@m, '[{server}]')")
                                                                      .WriteTo.File($"Logs/{server}/log-.txt",
                                                                        rollingInterval: RollingInterval.Day,
                                                                        rollOnFileSizeLimit: true,
                                                                        fileSizeLimitBytes: 10_485_760,
                                                                        retainedFileCountLimit: 14));
                                    
                                    common.Filter.ByExcluding($"StartsWith(@m, '[{server}]')");
                                }
                            });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/{server}/log", async Task (DiscordService service, LogData logData) => await service.LogWebHook(logData));
    app.MapGet("/{server}/chat", async Task (DiscordService service, ChatData chatData) => await service.ChatWebHook(chatData));

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