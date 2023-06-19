namespace ConanWebHooks.Models;

public class DiscordData
{
    public const string SectionName = nameof(DiscordData);

    public ServerHooks[] ServerHooks { get; set; } = Array.Empty<ServerHooks>();
}

public class ServerHooks
{
    public string Server { get; set; } = string.Empty;
    public bool SeparateLog { get; set; } = false;
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
    public string[] ExcludeCommands { get; set; } = Array.Empty<string>();
}