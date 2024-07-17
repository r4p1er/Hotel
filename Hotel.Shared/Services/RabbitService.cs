using System.Text;
using System.Text.Json;
using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hotel.Shared.Services;

public class RabbitService : IRabbitService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly RabbitOptions _options;
    
    public RabbitService(RabbitOptions options)
    {
        var factory = new ConnectionFactory { HostName = options.Host };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
        _options = options;

        _channel.QueueDeclare(
            queue: options.Queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public void Publish(string queue, RabbitMessage message)
    {
        var messageString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageString);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }

    public void AddMessageHandler(Func<RabbitMessage, Task> handler)
    {
        _consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<RabbitMessage>(messageString)!;

            await handler(message);
            
            _channel.BasicAck(ea.DeliveryTag, false);
        };
    }

    public void Listen()
    {
        _channel.BasicConsume(
            queue: _options.Queue,
            autoAck: false,
            consumer: _consumer
        );
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}