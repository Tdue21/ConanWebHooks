using Discord;

namespace SteamUpdateMonitor.Interfaces;

public interface IDiscordService
{
    Task SendMessageAsync(string message, IEnumerable<Embed>? embeds = null);
}