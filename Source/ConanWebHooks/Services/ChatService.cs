using ConanWebHooks.Models;
using Microsoft.Extensions.Options;

namespace ConanWebHooks.Services;

public class ChatService(ILogger<ChatService> logger, IOptions<DiscordData> options, WebHookService webHookService) : IReceiverService<ChatData>
{
    private readonly ServerHook[] _serverHooks = options?.Value?.ServerHooks ?? throw new ArgumentNullException(nameof(options));
    private readonly WebHookService _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
    private readonly ILogger<ChatService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task ReceiveData(ChatData data)
    {
        try
        {
            var serverHook = _serverHooks.FirstOrDefault(x => string.Equals(x.Server, data.Server, StringComparison.OrdinalIgnoreCase));
            if (serverHook != null)
            {
                var options = serverHook.ChatChannel;
                var hook = ulong.TryParse(options.Id, out var value) ? value : 0;
                var token = options.Token;
                var message = data.Text;

                if (hook != 0 && !string.IsNullOrWhiteSpace(token))
                {
                    if (options.MonitorChannels.Length == 0 || options.MonitorChannels.Contains(data.Channel))
                    {
                        var logToDiscord = options.ExcludeCommands.Length == 0 || options.ExcludeCommands.All(x => data.Message?.StartsWith(x) != true);
                        if (logToDiscord)
                        {
                            await _webHookService.SendMessageAsync(hook, token, message);
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
}

