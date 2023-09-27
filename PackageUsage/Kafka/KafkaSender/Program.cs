// See https://aka.ms/new-console-template for more information


using Confluent.Kafka;

var kafkaConfig = new ProducerConfig { BootstrapServers = "kafka地址:9092" };
Action<DeliveryReport<Null, string>> handler = r =>
    Console.WriteLine(!r.Error.IsError
        ? $"Delivered message to {r.TopicPartitionOffset}"
        : $"Delivery Error: {r.Error.Reason}");
using var p = new ProducerBuilder<Null, string>(kafkaConfig).Build();
p.Produce("ken-topic", new Message<Null, string> { Value = "ken-message" }, handler);
p.Flush(TimeSpan.FromSeconds(10));
