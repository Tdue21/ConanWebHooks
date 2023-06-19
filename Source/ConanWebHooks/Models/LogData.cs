using System.Reflection;

namespace ConanWebHooks.Models;

public class LogData
{
    public DateTime LogDate { get; set; }
    public string? Server { get; set; }
    public string? SteamId { get; set; }
    public string? CharacterName { get; set; }
    public string? ActName { get; set; }
    public string? EventId { get; set; }
    public string? EventCategory { get; set; }
    public string? EventType { get; set; }
    public string? ParameterData { get; set; }

    public string Text => $"[{LogDate}] {EventCategory} log: {CharacterName} triggered {EventId}: {ParameterData}";
    public string LogText => $"[{Server?.ToUpperInvariant()}] [{LogDate}] SteamId={SteamId}; CharacterName={CharacterName}; ActName={ActName}; EventId={EventId}; EventCategory={EventCategory}; EventType={EventType}; Parameters: {ParameterData}";

    public static ValueTask<LogData?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var server        = context.Request.RouteValues["server"];
        var logDate       = context.Request.Query["date"].ToDateTime();
        var steamId       = context.Request.Query["steamId"];
        var charName      = context.Request.Query["charName"];
        var actName       = context.Request.Query["actName"];
        var eventId       = context.Request.Query["eventId"];
        var eventCategory = context.Request.Query["eventCategory"];
        var eventType     = context.Request.Query["eventType"];
        var paramData     = context.Request.Query["params"];

        var result = new LogData
                     {
                         Server        = server?.ToString(), 
                         LogDate       = logDate,
                         SteamId       = steamId,
                         CharacterName = charName,
                         ActName       = actName,
                         EventId       = eventId,
                         EventCategory = eventCategory,
                         EventType     = eventType,
                         ParameterData = paramData
                     };
        return ValueTask.FromResult<LogData?>(result);
    }
}