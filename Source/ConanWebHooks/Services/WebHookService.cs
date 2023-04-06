using Discord.Webhook;

namespace ConanWebHooks.Services;

public class WebHookService
{
    public virtual async Task SendMessageAsync(ulong hookId, string token, string message)
    {
        using var client = new DiscordWebhookClient($"https://discord.com/api/webhooks/{hookId}/{token}");
        {
            await client.SendMessageAsync(message);
        }
    }
}