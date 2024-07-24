using System.Net;

namespace Hotel.Shared.Exceptions;

public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);