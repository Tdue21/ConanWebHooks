namespace ConanWebHooks.Models;

public class DiscordData
{
    public const string SectionName = nameof(DiscordData);

    public HookData? Server { get; set; }
    public HookData? LogChannel { get; set; }
    public HookChatData? ChatChannel { get; set; }
}

public class HookData
{
    public string? Id { get; set; }
    public string? Token { get; set; }
}

public class HookChatData : HookData
{
    public int[]? MonitorChannels { get; set; } = {1};
    public bool IncludeCommands { get; set; } = false;
}