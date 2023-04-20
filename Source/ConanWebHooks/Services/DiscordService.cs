using ConanWebHooks.Models;
using Microsoft.Extensions.Options;

namespace ConanWebHooks.Services;

public class DiscordService
{
    private readonly ILogger<DiscordService> _logger;
    private readonly WebHookService _webHookService;
    private readonly HookData _logOptions;
    private readonly HookChatData _chatOptions;
    
    public DiscordService(ILogger<DiscordService> logger, IOptions<DiscordData> options, WebHookService webHookService)
    {
        _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
        _logger         = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _chatOptions    = options?.Value?.ChatChannel ?? throw new ArgumentNullException(nameof(options.Value.ChatChannel));
        _logOptions     = options?.Value?.LogChannel ?? throw new ArgumentNullException(nameof(options.Value.LogChannel));
    }

    public async Task LogWebHook(LogData data)
    {
        try
        {
            var hook    = ulong.TryParse(_logOptions.Id, out var value) ? value : 0;
            var token   = _logOptions.Token;
            var message = data.Text;

            if (hook != 0 && !string.IsNullOrWhiteSpace(token))
            {
                await SendMessage(hook, token, message);
                _logger.LogInformation(data.LogText);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Log webhook");
        }
    }

    public async Task ChatWebHook(ChatData data)
    {
        try
        {
            var hook    = ulong.TryParse(_chatOptions.Id, out var value) ? value : 0;
            var token   = _chatOptions?.Token;
            var message = data.Text;

            if (hook != 0 && !string.IsNullOrWhiteSpace(token))
            {
                if (_chatOptions?.MonitorChannels.Length == 0 || _chatOptions?.MonitorChannels.Contains(data.Channel) == true)
                {
                    var logToDiscord = _chatOptions.IncludeCommands || data.Message?.StartsWith("/") == false;
                    if (logToDiscord)
                    {
                        await SendMessage(hook, token, message);
                    }
                }
                _logger.LogInformation(data.LogText);
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
