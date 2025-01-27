namespace ConanWebHooks.Models;

public class DiscordData
{
    public const string SectionName = nameof(DiscordData);

    public ServerHook[] ServerHooks { get; set; } = Array.Empty<ServerHook>();
}
