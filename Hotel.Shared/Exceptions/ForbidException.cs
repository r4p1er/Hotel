using System.Net;

namespace Hotel.Shared.Exceptions;

/// <summary>
/// Исключение со статус кодом 403 Forbidden
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class ForbidException(string message) : RequestException(HttpStatusCode.Forbidden, message);