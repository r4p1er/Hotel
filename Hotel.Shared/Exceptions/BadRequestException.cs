using System.Net;

namespace Hotel.Shared.Exceptions;

public class BadRequestException(string message) : RequestException(HttpStatusCode.BadRequest, message);