namespace ConanWebHooks.Services;

public class WindowsService : BackgroundService
{
    private readonly ILogger<WindowsService> _logger;
    private const string _serviceName = "Conan WebHooks Service";

    public WindowsService(ILogger<WindowsService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{_serviceName} is starting.");
        try
        {
            stoppingToken.Register(() => _logger.LogInformation($"{_serviceName} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{_serviceName} is doing background work.");
#if DEBUG
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
#endif
            }
        }
        catch (OperationCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            Environment.Exit(1);
        }

        _logger.LogInformation($"{_serviceName} has stopped.");
    }
}