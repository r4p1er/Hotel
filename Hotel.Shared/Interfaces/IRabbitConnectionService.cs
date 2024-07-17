using RabbitMQ.Client;

namespace Hotel.Shared.Interfaces;

public interface IRabbitConnectionService
{
    IModel CreateChannel();
}