namespace ConanWebHooks.Services;

public class WindowsService : BackgroundService
{
    private readonly ILogger<WindowsService> _logger;
    private const string ServiceName = "Conan WebHooks Service";

    public WindowsService(ILogger<WindowsService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{ServiceName} is starting.");

        stoppingToken.Register(() => _logger.LogInformation($"{ServiceName} is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"{ServiceName} is doing background work.");

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger.LogInformation($"{ServiceName} has stopped.");
    }
}