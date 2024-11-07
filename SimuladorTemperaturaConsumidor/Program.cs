using System;
using Confluent.Kafka;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            GroupId = "temperature-consumer-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        // Cargar el modelo ONNX
        using var session = new InferenceSession("temperature_model.onnx");
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        consumer.Subscribe("temperatura");

        while (true)
        {
            var consumeResult = consumer.Consume();
            float temperaturaActual = float.Parse(consumeResult.Message.Value);

            // Crear tensor de entrada para el modelo ONNX
            var inputTensor = new DenseTensor<float>(new[] { temperaturaActual }, new[] { 1, 1 });

            // Realizar la predicción
            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("float_input", inputTensor) };
            using var results = session.Run(inputs);
            float temperaturaPredicha = results.First().AsEnumerable<float>().First();

            Console.WriteLine($"Temperatura recibida: {temperaturaActual}°C | Predicción siguiente: {temperaturaPredicha}°C");
        }
    }
}
