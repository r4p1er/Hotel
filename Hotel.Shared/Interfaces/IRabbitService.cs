using Hotel.Shared.Models;

namespace Hotel.Shared.Interfaces;

public interface IRabbitService
{
    Task<RabbitMessage?> RequestMessageAsync(RabbitMessage message, int timeout = 15000);

    void SendMessage(RabbitMessage message);

    void Listen(Func<RabbitMessage, Task> handler);

    void Stop();

    void Close();
}