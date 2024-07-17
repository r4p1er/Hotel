using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using RabbitMQ.Client;

namespace Hotel.Shared.Services;

public class RabbitConnectionService : IRabbitConnectionService
{
    private readonly IConnection _connection;

    public RabbitConnectionService(RabbitOptions options)
    {
        var factory = new ConnectionFactory { HostName = options.Host };
        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel()
    {
        return _connection.CreateModel();
    }
}