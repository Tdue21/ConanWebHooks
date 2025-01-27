namespace ConanWebHooks.Models;

public class ServerHook
{
    public string Server { get; set; } = string.Empty;
    public bool SeparateLog { get; set; } = false;
    public LogHookData LogChannel { get; set; } = new();
    public ChatHookData ChatChannel { get; set; } = new();
    public HookData SpawnChannel { get; set; } = new();

}
