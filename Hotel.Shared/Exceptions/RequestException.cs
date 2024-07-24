using System.Net;

namespace Hotel.Shared.Exceptions;

/// <summary>
/// Базовый класс для создания кастомных исключений с HTTP статус кодами
/// </summary>
/// <param name="statusCode">HTTP статус код</param>
/// <param name="message">Сообщение исключения</param>
public class RequestException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}