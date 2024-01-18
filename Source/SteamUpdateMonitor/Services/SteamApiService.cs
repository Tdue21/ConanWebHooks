using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamUpdateMonitor.Interfaces;
using SteamUpdateMonitor.Models;

namespace SteamUpdateMonitor.Services;

/// <summary>
/// 
/// </summary>
public class SteamApiService : ISteamApiService
{
    private const string SteamApiBaseUrl = "https://api.steampowered.com";
    private readonly ILogger<SteamApiService> _logger;
    private readonly SteamSettings _settings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="settings"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SteamApiService(ILogger<SteamApiService> logger, IOptions<SteamSettings> settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc/>
    public async Task<QueryFilesResult> QueryApplication(string appId, int pageNo, int numPerPage, CancellationToken cancellationToken)
    {
        try
        {
            var result = await SteamApiBaseUrl
                        .AppendPathSegments("IPublishedFileService", "QueryFiles", "v1")
                        .SetQueryParams(new
                        {
                            key = _settings.ApiKey,
                            format = "json",
                            query_type = 21,
                            appid = appId,
                            return_metadata = 1,
                            return_short_description = 1,
                            page = pageNo,
                            numperpage = numPerPage
                        })
                        .GetJsonAsync<QueryFilesResult>(cancellationToken: cancellationToken);
            return result;
        }
        catch (FlurlHttpException ex)
        {
            var err = await ex.GetResponseStringAsync();
            _logger.LogError("Steam query files failed with status code: {0} - {1}.", ex.StatusCode, err);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<GetPublishedFilesResult> QueryModList(string[] modIds, CancellationToken cancellationToken)
    {
        try
        {
            var content = new Dictionary<string, object>
            {
                { "key", _settings.ApiKey! },
                { "format", "json" },
                { "itemcount", modIds.Length }
            };

            for(var idx = 0; idx < modIds.Length; idx++)
            {
                content.Add($"publishedfileIds[{idx}]", modIds[idx]);
            }

            var result = await SteamApiBaseUrl
                        .AppendPathSegments("ISteamRemoteStorage", "GetPublishedFileDetails", "v1")
                        .PostUrlEncodedAsync(content, cancellationToken: cancellationToken)
                        .ReceiveJson<GetPublishedFilesResult>();
            return result;
        }
        catch (FlurlHttpException ex)
        {
            var err = await ex.GetResponseStringAsync();
            _logger.LogError("Steam query files failed with status code: {0} - {1}.", ex.StatusCode, err);
            throw;
        }

    }
}

