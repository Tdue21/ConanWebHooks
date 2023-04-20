namespace ConanWebHooks.Models;

public class DiscordData
{
    public const string SectionName = nameof(DiscordData);

    public HookData Server { get; set; } = new();
    public HookData LogChannel { get; set; } = new();
    public HookChatData ChatChannel { get; set; } = new();
}

public class HookData
{
    public string Id { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public class HookChatData : HookData
{
    public int[] MonitorChannels { get; set; } = Array.Empty<int>();
    public bool IncludeCommands { get; set; } = false;
}