using System.Reflection;
using System.Threading.Channels;

namespace ConanWebHooks.Models;

public class SpawnData
{
    private DateTime _received;

    public string? Server { get; init; }
    public string? CharName { get; init; }
    public string? SteamId { get; init; }
    public string? Date { get; init; }
    public string? EventId { get; init; }
    public string? EventType { get; init; }
    public string? Params { get; init; }

    public string Text => $"[{_received.TimeOfDay:hh\\:mm\\:ss}] **{CharName}** ({SteamId}) has joined the server.";
    //public string LogText => $"[{Server?.ToUpperInvariant()}] [{_received.TimeOfDay:hh\\:mm\\:ss}] Character={Character}; Sender={Sender}; Channel={Channel}; Radius={Radius}; Location={Location}; Message={Message}";

    public static ValueTask<SpawnData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        // Request starting HTTP / 1.1 GET http://192.168.0.41:5000/sinners/spawn
        // ?charName=Corathine
        // &steamId=76561198019948713
        // &date=2025%2D01%2D27%2017%3A07%3A37
        // &eventId=401B006D491924EB913AB495BAA88F23
        // &eventCategory=Tot%21Admin
        // &eventType=InScript
        // &params=Corathine - null 0

        var server = context.Request.RouteValues["server"];
        var charName = context.Request.Query["charName"];
        var steamId = context.Request.Query["steamId"];
        var date = context.Request.Query["date"];
        var eventId = context.Request.Query["eventId"];
        var eventType = context.Request.Query["eventType"];
        var queryParams = context.Request.Query["params"];

        var result = new SpawnData
        {
            Server = server?.ToString(),
            _received = DateTime.Now,
            CharName = charName,
            SteamId = steamId,
            Date = date,
            EventId = eventId,
            EventType = eventType,
            Params = queryParams
        };

        return ValueTask.FromResult<SpawnData?>(result);
    }

}