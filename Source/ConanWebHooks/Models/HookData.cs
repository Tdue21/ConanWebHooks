namespace ConanWebHooks.Models;

public class Settings
{
    public Dictionary<string, ServerData> Servers { get; set; } = new();
}

public class ServerData
{
    public bool SeparateLog { get; set; }
    public LogHookData? LogChannel { get; set; }
    public ChatHookData? ChatChannel { get; set; }
    public HookData? SpawnChannel { get; set; }
}

public record HookData (string Id, string Token);

public record LogHookData(string Id, string Token, bool ParseLog) : HookData(Id, Token);

public record ChatHookData(string Id, string Token, int[] MonitorChannels, string[] ExcludeCommands) : HookData(Id, Token);
