using ConanWebHooks.Models;
using Microsoft.Extensions.Options;

namespace ConanWebHooks.Services;

public class DiscordService
{
    private readonly ILogger<DiscordService> _logger;
    private readonly WebHookService _webHookService;
    private readonly ServerHook[] _serverHooks;

    public DiscordService(ILogger<DiscordService> logger, IOptions<DiscordData> options, WebHookService webHookService)
    {
        _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serverHooks = options.Value.ServerHooks ?? throw new ArgumentNullException(nameof(options.Value.ServerHooks));
    }

    public async Task LogWebHook(LogData data)
    {
        try
        {
            var serverHook = _serverHooks.FirstOrDefault(x => string.Equals(x.Server, data.Server, StringComparison.OrdinalIgnoreCase));
            if (serverHook != null)
            {
                var options = serverHook.LogChannel;

                var hook = ulong.TryParse(options.Id, out var value) ? value : 0;
                var token = options.Token;
                var message = data.Text;

                if (hook != 0 && !string.IsNullOrWhiteSpace(token))
                {
                    if(options.ParseLog)
                    {
                        await ParseLog(hook, token, data);
                    }
                    else
                    {
                        await SendMessage(hook, token, message);
                    }
                    _logger.LogInformation(data.LogText);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "WebHook::Log");
        }
    }

    public async Task ChatWebHook(ChatData data)
    {
        try
        {
            var serverHook = _serverHooks.FirstOrDefault(x => string.Equals(x.Server, data.Server, StringComparison.OrdinalIgnoreCase));
            if (serverHook != null)
            {
                var options = serverHook.ChatChannel;
                var hook    = ulong.TryParse(options.Id, out var value) ? value : 0;
                var token   = options.Token;
                var message = data.Text;

                if (hook != 0 && !string.IsNullOrWhiteSpace(token))
                {
                    if (options.MonitorChannels.Length == 0 || options.MonitorChannels.Contains(data.Channel))
                    {
                        var logToDiscord = options.ExcludeCommands.All(x => data.Message?.StartsWith(x) != true);
                        if (logToDiscord)
                        {
                            await SendMessage(hook, token, message);
                        }
                    }
                    _logger.LogInformation(data.LogText);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "WebHook::Chat");
        }
    }

    private async Task SendMessage(ulong hook, string token, string message)
    {
        await _webHookService.SendMessageAsync(hook, token, message);
    }

    private async Task ParseLog(ulong hook, string token, LogData data)
    {
        if(data.EventType == "FlowChart")
        {
            var parameters = data.ParameterData;
        }


    }
}
