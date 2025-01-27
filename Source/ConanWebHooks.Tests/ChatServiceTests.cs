using ConanWebHooks.Models;
using ConanWebHooks.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace ConanWebHooks.Tests;

public class ChatServiceTests
{
    private ILogger<ChatService> _logger;
    private IOptions<DiscordData> _options;
    private WebHookService _webHookService;

    [Fact]
    public async Task ReceiveDataTestAsync()
    {
        _logger = Substitute.For<ILogger<ChatService>>();
        _options = Substitute.For<IOptions<DiscordData>>();
        _options.Value.Returns(GetDiscordData());

        _webHookService = Substitute.For<WebHookService>();

        var service = new ChatService(_logger, _options, _webHookService);

        await service.ReceiveData(new ChatData
        {
            Server = "Test",
            Channel = 1,
            Character = "TestCharacter",
            Location = "TestLocation",
            Message = "TestMessage",
            Radius = "TestRadius",
            Sender = "TestSender"
        });

        await _webHookService.Received(1).SendMessageAsync(Arg.Any<ulong>(), Arg.Any<string>(), Arg.Any<string>());

    }

    private DiscordData GetDiscordData()
    {
        return new DiscordData
        {
            ServerHooks =
            [
                new ServerHook
                {
                    Server = "Test",
                    ChatChannel = new ChatHookData
                    {
                        Id = "123456789",
                        Token = "TestToken",
                        MonitorChannels = [1, 2],
                        ExcludeCommands = []
                    }
                }
            ]
        };
    }
}