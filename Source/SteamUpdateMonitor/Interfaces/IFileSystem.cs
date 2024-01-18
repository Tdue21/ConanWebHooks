using System.Text;

namespace SteamUpdateMonitor.Interfaces;

/// <summary>
/// Abstraction interface for file system operations. 
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<string> ReadFileAsync(string file, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    Task<string> ReadFileAsync(string file, Encoding encoding, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteFileAsync(string file, string content, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="content"></param>
    /// <param name="encoding"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteFileAsync(string file, string content, Encoding encoding, CancellationToken cancellationToken);
}