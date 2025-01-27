
using ConanWebHooks.Models;
using Microsoft.Extensions.Options;

namespace ConanWebHooks.Services;

public class SpawnService(ILogger<SpawnService> logger, IOptions<DiscordData> options, WebHookService webHookService) : IReceiverService<SpawnData>
{
    private readonly ServerHook[] _serverHooks = options?.Value?.ServerHooks ?? throw new ArgumentNullException(nameof(options));
    private readonly WebHookService _webHookService = webHookService ?? throw new ArgumentNullException(nameof(webHookService));
    private readonly ILogger<SpawnService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task ReceiveData(SpawnData data)
    {
        try
        {
            var serverHook = _serverHooks.FirstOrDefault(x => string.Equals(x.Server, data.Server, StringComparison.OrdinalIgnoreCase));
            if (serverHook != null)
            {
                var options = serverHook.SpawnChannel;
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

