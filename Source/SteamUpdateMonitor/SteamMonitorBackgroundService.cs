using System.Text;
using Discord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SteamUpdateMonitor.Interfaces;
using SteamUpdateMonitor.Models;

namespace SteamUpdateMonitor;

internal class SteamMonitorBackgroundService : BackgroundService
{
    private readonly ILogger<SteamMonitorBackgroundService> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly ISteamApiService _steamApiService;
    private readonly IDiscordService _discordService;
    private readonly ApplicationSettings _settings;
    private DateTime _nextRun;

    public SteamMonitorBackgroundService(ISteamApiService steamApiService,
                                         IDiscordService discordService,
                                         IOptions<ApplicationSettings> settings,
                                         ILogger<SteamMonitorBackgroundService> logger,
                                         IFileSystem fileSystem)
    {
        _steamApiService = steamApiService ?? throw new ArgumentNullException(nameof(steamApiService));
        _discordService = discordService ?? throw new ArgumentNullException(nameof(discordService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _nextRun = DateTime.MinValue;
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_nextRun < DateTime.Now)
            {
                await PerformModUpdateCheck(stoppingToken);
                _nextRun = DateTime.Now.AddMinutes(_settings.Frequency);
            }
            await Task.Delay(10 * 1000, stoppingToken);
        }
    }

    private async Task PerformModUpdateCheck(CancellationToken cancellationToken)
    {
        try
        {
            var ids = _settings.ModIds;
            var modUpdates = await GetLastUpdated(cancellationToken);
            var data = await GetWorkshopData(ids, cancellationToken);

            var isUpdated = new List<ModDetails>();
            if (data != null && modUpdates != null)
            {
                foreach (var item in data)
                {
                    if (!string.IsNullOrWhiteSpace(item.WorkshopId))
                    {
                        if (modUpdates.TryGetValue(item.WorkshopId, out var lastTime))
                        {
                            if (lastTime < item.Updated)
                            {
                                isUpdated.Add(item);
                                modUpdates[item.WorkshopId] = item.Updated.Value;
                            }
                        }
                        else
                        {
                            modUpdates.Add(item.WorkshopId, item.Updated.Value);
                        }
                    }
                }
            }

            await SaveLastUpdated(modUpdates, cancellationToken);

            if (isUpdated.Count > 0)
            {
                await SendUpdateToDiscord(isUpdated);
            }

            _logger.LogInformation("Checked {modCount} mods. {isUpdated} has updates.", ids.Length, isUpdated.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to complete action.");
        }
    }

    private async Task SendUpdateToDiscord(List<ModDetails> isUpdated)
    {
        var message = new StringBuilder()
            .AppendLine("@admin @here")
            .AppendLine($"**There are {isUpdated.Count} mod update(s):**")
            .AppendLine("---");

        isUpdated.ForEach(x => message.AppendLine($"* [{x.Title}](https://steamcommunity.com/sharedfiles/filedetails/?id={x.WorkshopId}"));

        await _discordService.SendMessageAsync(message.ToString());
    }

    private async Task<ModDetails[]?> GetWorkshopData(string[] ids, CancellationToken cancellationToken)
    {
        var result = await _steamApiService.QueryModList(ids, cancellationToken);
        var data = result?.Response?
                          .PublishedFileDetails?
                          .Select(x => new ModDetails
                          {
                              WorkshopId = x.publishedfileid,
                              Title = x.title,
                              OwnerId = x.creator,
                              Created = DateTimeOffset.FromUnixTimeSeconds(x.time_created),
                              Updated = DateTimeOffset.FromUnixTimeSeconds(x.time_updated)
                          })
                          .ToArray();
        return data;
    }

    private async Task<Dictionary<string, DateTimeOffset>?> GetLastUpdated(CancellationToken cancellationToken)
    {
        var times = await _fileSystem.ReadFileAsync("mods.json", cancellationToken: cancellationToken);
        var modUpdates = JsonConvert.DeserializeObject<Dictionary<string, DateTimeOffset>>(times);
        return modUpdates;
    }

    private async Task SaveLastUpdated(Dictionary<string, DateTimeOffset>? modUpdates, CancellationToken cancellationToken)
    {
        var content = JsonConvert.SerializeObject(modUpdates, Formatting.Indented);
        await _fileSystem.WriteFileAsync("mods.json", content, cancellationToken);
    }
}