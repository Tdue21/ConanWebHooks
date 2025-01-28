using ConanWebHooks.Models;
using ConanWebHooks.Services;

namespace ConanWebHooks.Logic;

public class SpawnHandler(ILogger<SpawnHandler> logger, SettingsService settingsService, WebHookService webHookService) : IReceiverService<SpawnQueryModel>
{
    private readonly WebHookService _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
    private readonly SettingsService _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    private readonly ILogger<SpawnHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task ReceiveData(SpawnQueryModel data)
    {
        try
        {
            var settings = await _settingsService.GetSettings();
            if (!string.IsNullOrWhiteSpace(data.Server) && settings.Servers.TryGetValue(data.Server, out var server))
            {
                var options = server.SpawnChannel;
                var hook = ulong.TryParse(options.Id, out var value) ? value : 0;
                var token = options.Token;
                var message = data.Text;

                if (hook != 0 && !string.IsNullOrWhiteSpace(token))
                {
                    await _webHookService.SendMessageAsync(hook, token, message);
                    _logger.LogInformation(data.Text);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "WebHook::Spawn");
        }
    }
}

