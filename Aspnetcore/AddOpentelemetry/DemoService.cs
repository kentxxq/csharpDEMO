namespace AddOpenTelemetry;

public class DemoService:IDemoService
{
    private readonly ILogger<DemoService> _logger;

    public DemoService(ILogger<DemoService> logger)
    {
        _logger = logger;
    }

    public string GetData()
    {
        _logger.LogInformation("log-demo-data");
        return "demo data";
    }
}