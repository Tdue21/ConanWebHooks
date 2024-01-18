namespace SteamUpdateMonitor.Interfaces;

public interface IDiscordService
{
    Task SendMessageAsync(string message);
}