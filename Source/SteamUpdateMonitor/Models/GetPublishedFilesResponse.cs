namespace SteamUpdateMonitor.Models;

public class GetPublishedFilesResponse
{
    public int Result { get; set; }
    public int ResultCount { get; set; }
    public SimplePublishedFileDetail[]? PublishedFileDetails { get; set; }
}