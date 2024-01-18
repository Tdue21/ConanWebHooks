namespace SteamUpdateMonitor.Models;

public class QueryFilesResponse
{
    public int Total { get; set; }
    public PublishedFileDetail[]? PublishedFileDetails { get; set; }
}

