using System;
using System.Threading.Tasks;
using Confluent.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        Random random = new Random();

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            while (true)
            {
                var temperatura = random.Next(15, 30).ToString();
                await producer.ProduceAsync("temperatura", new Message<Null, string> { Value = temperatura });
                Console.WriteLine($"Temperatura enviada: {temperatura}°C");
                await Task.Delay(2000);
            }
        }
    }
}
