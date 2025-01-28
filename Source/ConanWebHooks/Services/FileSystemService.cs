namespace ConanWebHooks.Services;


public interface IDataService
{
    Task<string> ReadData(string path);
}

public class FileSystemService : IDataService
{
    public async Task<string> ReadData(string path) => await File.ReadAllTextAsync(path);

}
