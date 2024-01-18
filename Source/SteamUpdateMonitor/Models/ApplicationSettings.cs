namespace SteamUpdateMonitor.Models;

/// <summary>
/// 
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// 
    /// </summary>
    public int Frequency { get; set; } = 60;
    /// <summary>
    /// 
    /// </summary>
    public string[] ModIds { get; set; } = Array.Empty<string>();
}
