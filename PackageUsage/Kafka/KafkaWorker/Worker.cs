using Confluent.Kafka;

namespace KafkaWorker;

public class Worker : BackgroundService
{
    private static readonly ConsumerConfig KafkaConfig = new() {
        BootstrapServers = "localhost:9092", GroupId = "ken-gid", AutoOffsetReset = AutoOffsetReset.Earliest
    };

    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        using var c = new ConsumerBuilder<Ignore, string>(KafkaConfig).Build();
        c.Subscribe("ken-topic");
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = c.Consume(stoppingToken);
                    _logger.LogInformation("Consumed message '{Value}' at: '{TopicPartitionOffset}'", cr.Message.Value,
                        cr.TopicPartitionOffset);
                }
                catch (ConsumeException e)
                {
                    _logger.LogInformation("Error occured: {Reason}", e.Error.Reason);
                }
            }
        }
        catch (OperationCanceledException)
        {
            c.Close();
        }
    }
}
