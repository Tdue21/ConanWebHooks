namespace ConanWebHooks.Models;

public class ChatHookData : HookData
{
    public int[] MonitorChannels { get; set; } = Array.Empty<int>();
    public string[] ExcludeCommands { get; set; } = Array.Empty<string>();
}
