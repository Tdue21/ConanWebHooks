using ConanWebHooks.Models;
using ConanWebHooks.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    if (builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddUserSecrets<DiscordData>(true);
    }

    builder.Services.Configure<DiscordData>(builder.Configuration.GetSection(DiscordData.SectionName));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<WebHookService>();
    builder.Services.AddTransient<DiscordService>();

    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/log", async Task (DiscordService service, LogData logData) => await service.LogWebHook(logData));
    app.MapGet("/chat", async Task (DiscordService service, ChatData chatData) => await service.ChatWebHook(chatData));

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}