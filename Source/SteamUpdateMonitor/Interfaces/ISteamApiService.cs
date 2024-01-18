using SteamUpdateMonitor.Models;

namespace SteamUpdateMonitor.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ISteamApiService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="pageNo"></param>
    /// <param name="numPerPage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueryFilesResult> QueryApplication(string appId, int pageNo, int numPerPage, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetPublishedFilesResult> QueryModList(string[] modIds, CancellationToken cancellationToken);
}
