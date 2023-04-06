using System.Reflection;

namespace ConanWebHooks.Models;

public class ChatData
{
    public DateTime Received { get; init; }
    public string? Message { get; init; }
    public string? Sender { get; init; }
    public string? Character { get; init; }
    public string? Radius { get; init; }
    public string? Location { get; init; }
    public int Channel { get; init; }

    public string Text => $"[{Received.TimeOfDay:hh\\:mm\\:ss}] **{Character}** in channel '{Channel}': {Message}";
    public string LogText => $"[{Received.TimeOfDay:hh\\:mm\\:ss}] Character={Character}; Sender={Sender}; Channel={Channel}; Radius={Radius}; Location={Location}; Message={Message}";

    public static ValueTask<ChatData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {

        // message, sender, character, radius, location, channel
        var message   = context.Request.Query["message"];
        var sender    = context.Request.Query["sender"];
        var character = context.Request.Query["character"];
        var radius    = context.Request.Query["radius"];
        var location  = context.Request.Query["location"];
        var channel   = context.Request.Query["channel"].ToInt32();

        var result = new ChatData
                     {
                         Received  = DateTime.Now,
                         Message   = message,
                         Sender    = sender,
                         Character = character,
                         Radius    = radius,
                         Location  = location,
                         Channel   = channel
                     };
        return ValueTask.FromResult<ChatData?>(result);
    }
}