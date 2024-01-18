using System.Diagnostics;

namespace SteamUpdateMonitor.Models;

[DebuggerDisplay("{WorkshopId}; {OwnerId}; {Title}; {Created}; {Updated}")]
public class ModDetails
{
    public string? WorkshopId { get; set; }
    public string? OwnerId { get; set;}
    public string? Title { get; set; }
    public DateTimeOffset? Created { get; set; }
    public DateTimeOffset? Updated { get; set;}
}