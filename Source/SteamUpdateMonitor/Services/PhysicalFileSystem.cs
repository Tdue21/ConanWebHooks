using System.Text;
using SteamUpdateMonitor.Interfaces;
using SteamUpdateMonitor.Models;

namespace SteamUpdateMonitor.Services;

/// <summary>
/// Concrete implementation of the <see cref="IFileSystem"/> interface.
/// </summary>
public class PhysicalFileSystem : IFileSystem
{
    /// <inheritdoc />
    public ModFileInfo[] GetFileInfo(string path)
    {
        var files = Directory.EnumerateFiles(path, "*.pak", SearchOption.AllDirectories)
                             .Select(file =>
                             {
                                 var updated = new DateTimeOffset(new FileInfo(file).LastWriteTimeUtc);
                                 var filePath = Path.GetDirectoryName(file);
                                 var directory = new DirectoryInfo(filePath!).Name;
                                 var fileInfo = new ModFileInfo { ModId = directory, Updated = updated };

                                 return fileInfo;
                             })
                             .ToArray();
        return files;
    }

    /// <inheritdoc />
    public Task<string> ReadFileAsync(string file, CancellationToken cancellationToken) 
        => ReadFileAsync(file, Encoding.UTF8, cancellationToken);

    /// <inheritdoc />
    public Task<string> ReadFileAsync(string file, Encoding encoding, CancellationToken cancellationToken) 
        => File.ReadAllTextAsync(Path.GetFullPath(file), encoding, cancellationToken);

    /// <inheritdoc />
    public Task WriteFileAsync(string file, string content, CancellationToken cancellationToken) 
        => WriteFileAsync(file, content, Encoding.UTF8, cancellationToken);

    /// <inheritdoc />
    public Task WriteFileAsync(string file, string content, Encoding encoding, CancellationToken cancellationToken) 
        => File.WriteAllTextAsync(Path.GetFullPath(file), content, encoding, cancellationToken);
}