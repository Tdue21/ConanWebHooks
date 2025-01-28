using ConanWebHooks.Models;
using ConanWebHooks.Services;

namespace ConanWebHooks.Logic;

public class ChatHandler(ILogger<ChatHandler> logger, SettingsService settingsService, WebHookService webHookService) : IReceiverService<ChatQueryModel>
{
    private readonly ILogger<ChatHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly SettingsService _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    private readonly WebHookService _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));

    public async Task ReceiveData(ChatQueryModel data)
    {
        try
        {
            var settings = await _settingsService.GetSettings();
            if (!string.IsNullOrWhiteSpace(data.Server) && settings.Servers.TryGetValue(data.Server, out var server))
            {
                var options = server.ChatChannel;
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

