using Discord;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using SteamUpdateMonitor.Models;
using SteamUpdateMonitor.Services;
using System.Linq;
using System.Numerics;

namespace SteamUpdateMonitor.Tests
{
    public class SteamApiServiceIntegrationTests
    {
        private readonly SteamApiService _steam;
  
        public SteamApiServiceIntegrationTests()
        {
            var steamSettings = new SteamSettings { ApiKey = "INSERT STEAM API KEY HERE" };
            var settings = Substitute.For<IOptions<SteamSettings>>();
            settings.Value.Returns(steamSettings);
            var logger = Substitute.For<ILogger<SteamApiService>>();
            _steam = new SteamApiService(logger, settings);

        }

        [Fact]
        public async Task QueryApplication_Test()
        {

            var allMods = new List<ModDetails>();
            var hasRemaining = true;
            var pageNo = 1;
            var total = 0;
            while (hasRemaining)
            {
                var result = await _steam.QueryApplication("440900", pageNo, 100, CancellationToken.None);
                total = result?.Response?.Total ?? 0;
                var items = result?.Response?.PublishedFileDetails?.Select(x => new ModDetails
                {
                    WorkshopId = x.publishedfileid,
                    OwnerId = x.creator,
                    Title = x.title,
                    Created = DateTimeOffset.FromUnixTimeSeconds(x.time_created).DateTime,
                    Updated = DateTimeOffset.FromUnixTimeSeconds(x.time_updated).DateTime
                });

                if (items != null)
                {
                    allMods.AddRange(items);
                }

                hasRemaining = allMods.Count < total;
                pageNo++;
            }

            allMods.Count.Should().Be(total);
        }

        [Fact]
        public async Task QueryModList_Test()
        {
            var result = await _steam.QueryModList(modIds, CancellationToken.None);

            var items = result?.Response?.PublishedFileDetails?.Select(x => new ModDetails
            {
                WorkshopId = x.publishedfileid,
                OwnerId = x.creator,
                Title = x.title,
                Created = DateTimeOffset.FromUnixTimeSeconds(x.time_created).DateTime,
                Updated = DateTimeOffset.FromUnixTimeSeconds(x.time_updated).DateTime
            });

            items.Should().HaveCount(modIds.Length);
        }

        private readonly static string[] modIds =
        [
            "1823412793",
            "3036057084",
            "2846119484",
            "3036058836",
            "2847709656",
            "2886779102",
            "2974559563",
            "1797359985",
            "3116472449"
        ];
    }
}