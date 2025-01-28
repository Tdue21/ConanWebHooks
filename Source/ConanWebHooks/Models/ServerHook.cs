namespace ConanWebHooks.Models;

public class ServerHook
{
    public string Server { get; set; } = string.Empty;
    public bool SeparateLog { get; set; } = false;
    public LogHookData LogChannel { get; set; }
    public ChatHookData ChatChannel { get; set; }
    public HookData SpawnChannel { get; set; }

}
