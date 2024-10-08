using System.Net;

namespace Hotel.Shared.Exceptions;

/// <summary>
/// Исключение со статус кодом 400 Bad Request
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class BadRequestException(string message) : RequestException(HttpStatusCode.BadRequest, message);