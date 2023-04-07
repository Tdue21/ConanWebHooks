using ConanWebHooks.Models;
using Microsoft.Extensions.Options;

namespace ConanWebHooks.Services;

public class DiscordService
{
    private readonly ILogger<DiscordService> _logger;
    private readonly WebHookService _webHookService;
    private readonly DiscordData _options;

    public DiscordService(ILogger<DiscordService> logger, IOptions<DiscordData> options, WebHookService webHookService)
    {
        _logger         = logger ?? throw new ArgumentNullException(nameof(logger));
        _options        = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
    }

    public async Task LogWebHook(LogData? data)
    {
        try
        {
            var hook    = ulong.TryParse(_options.LogChannel?.Id, out var value) ? value : 0;
            var token   = _options.LogChannel?.Token;
            var message = data?.Text;

            if (hook != 0 && !string.IsNullOrWhiteSpace(token))
            {
                await SendMessage(hook, token, message!);
                _logger.LogInformation(data?.LogText);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Log webhook");
        }
    }

    public async Task ChatWebHook(ChatData? data)
    {
        try
        {
            var hook    = ulong.TryParse(_options.ChatChannel?.Id, out var value) ? value : 0;
            var token   = _options.ChatChannel?.Token;
            var message = data?.Text;

            if (hook != 0 && !string.IsNullOrWhiteSpace(token))
            {
                if (_options.ChatChannel?.MonitorChannels?.Contains(data?.Channel ?? 0) == true)
                {
                    var logToDiscord = _options.ChatChannel.IncludeCommands || data?.Message?.StartsWith("/") == false;
                    if (logToDiscord)
                    {
                        await SendMessage(hook, token, message!);
                    }
                }
                _logger.LogInformation(data?.LogText);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Chat webhook");
        }
    }

    private async Task SendMessage(ulong hook, string token, string message)
    {
        await _webHookService.SendMessageAsync(hook, token, message);
    }
}
