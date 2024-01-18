using System.Text;
using SteamUpdateMonitor.Interfaces;

namespace SteamUpdateMonitor.Services;

/// <summary>
/// Concrete implementation of the <see cref="IFileSystem"/> interface.
/// </summary>
public class PhysicalFileSystem : IFileSystem
{
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