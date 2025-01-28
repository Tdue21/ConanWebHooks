using ConanWebHooks.Logic;
using ConanWebHooks.Models;
using ConanWebHooks.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ConanWebHooks.Tests;

public class ChatServiceTests
{
    private ILogger<ChatHandler> _logger;
    private SettingsService _options;
    private WebHookService _webHookService;

    [Fact]
    public async Task ReceiveDataTestAsync()
    {
        _logger = Substitute.For<ILogger<ChatHandler>>();
        _options = Substitute.For<SettingsService>();
        _options.GetSettings().Returns(GetDiscordData());

        _webHookService = Substitute.For<WebHookService>();

        var service = new ChatHandler(_logger, _options, _webHookService);

        await service.ReceiveData(new ChatQueryModel
        {
            Server = "Test",
            Channel = 1,
            Character = "TestCharacter",
            Location = "TestLocation",
            Message = "TestMessage",
            Radius = "TestRadius",
            Sender = "TestSender"
        });

        await _webHookService.Received(1)
                             .SendMessageAsync(Arg.Is<ulong>(x => x == 123456789), 
                                               Arg.Is<string>(x => x == "TestToken"), 
                                               Arg.Is<string>(x => x == ));

    }

    private Settings GetDiscordData()
    {
        return new()
        {
            Servers = new Dictionary<string, ServerData>
            {
                ["Test"] = new ServerData
                {
                    ChatChannel = new ChatHookData
                    {
                        Id = "123456789",
                        Token = "TestToken",
                        MonitorChannels = [1, 2],
                        ExcludeCommands = []
                    }
                }
            }
        };
    }
}