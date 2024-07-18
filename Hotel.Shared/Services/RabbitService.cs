using System.Text;
using System.Text.Json;
using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Timer = System.Timers.Timer;

namespace Hotel.Shared.Services;

public class RabbitService : IRabbitService
{
    private readonly IModel _channel;
    private readonly RabbitOptions _options;
    private string _consumerTag = string.Empty;
    private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };
    
    public RabbitService(IRabbitConnectionService connection, RabbitOptions options)
    {
        _channel = connection.CreateChannel();
        _options = options;

        _channel.QueueDeclare(
            queue: options.Queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public async Task<RabbitMessage?> RequestMessageAsync(RabbitMessage message, int timeout = 15000)
    {
        var queueName = _channel.QueueDeclare().QueueName;
        var consumer = new EventingBasicConsumer(_channel);
        RabbitMessage? result = null;
        
        consumer.Received += (sender, args) =>
        {
            var responseBody = args.Body.ToArray();
            var responseBodyString = Encoding.UTF8.GetString(responseBody);
            result = JsonSerializer.Deserialize<RabbitMessage>(responseBodyString, _serializerOptions)!;
            (sender as EventingBasicConsumer)!.Model.BasicAck(args.DeliveryTag, false);
        };

        var consumerTag = _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );

        message.ResponseTarget = queueName;
        var messageString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageString);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: message.Receiver,
            basicProperties: null,
            body: body
        );

        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        using var timer = new Timer(timeout);
        timer.AutoReset = false;
        timer.Elapsed += (sender, args) => tokenSource.Cancel();
        timer.Enabled = true;
        
        await Task.Run(() =>
        {
            while (result == null && !token.IsCancellationRequested)
            {
            }
        }, token);
        
        _channel.BasicCancel(consumerTag);
        _channel.QueueDelete(queueName);

        return result;
    }

    public void SendMessage(RabbitMessage message)
    {
        var messageString = JsonSerializer.Serialize(message)!;
        var body = Encoding.UTF8.GetBytes(messageString);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: message.Receiver,
            basicProperties: null,
            body: body
        );
    }

    public void Listen(Func<RabbitMessage, Task> handler)
    {
        if (!string.IsNullOrWhiteSpace(_consumerTag)) return;
        
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<RabbitMessage>(messageString, _serializerOptions)!;

            await handler(message);

            (sender as EventingBasicConsumer)!.Model.BasicAck(args.DeliveryTag, false);
        };
        
        _consumerTag = _channel.BasicConsume(
            queue: _options.Queue,
            autoAck: false,
            consumer: consumer
        );
    }

    public void Stop()
    {
        if (string.IsNullOrWhiteSpace(_consumerTag)) return;
        
        _channel.BasicCancel(_consumerTag);
        _consumerTag = string.Empty;
    }

    public void Close()
    {
        Stop();
        _channel.Close();
    }
}