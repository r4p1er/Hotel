using System.Net;

namespace Hotel.Shared.Exceptions;

/// <summary>
/// Исключение со статуст кодом 404 Not Found
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);