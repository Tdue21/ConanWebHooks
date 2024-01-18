using Discord.Webhook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamUpdateMonitor.Interfaces;
using SteamUpdateMonitor.Models;

namespace SteamUpdateMonitor.Services;

internal class DiscordService : IDiscordService
{
    private readonly ILogger<DiscordService> _logger;
    private readonly DiscordSettings _settings;

    public DiscordService(ILogger<DiscordService> logger, IOptions<DiscordSettings> settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public virtual async Task SendMessageAsync(string message)
    {
        try
        {
            var hookId = _settings.Id;
            var token = _settings.Token;

            using var client = new DiscordWebhookClient($"https://discord.com/api/webhooks/{hookId}/{token}");
            {
                await client.SendMessageAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to message Discord");
        }
    }
}
