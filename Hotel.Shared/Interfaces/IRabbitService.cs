using Hotel.Shared.Models;

namespace Hotel.Shared.Interfaces;

public interface IRabbitService : IDisposable
{
    void Publish(string queue, RabbitMessage message);

    void AddMessageHandler(Func<RabbitMessage, Task> handler);

    void Listen();
}