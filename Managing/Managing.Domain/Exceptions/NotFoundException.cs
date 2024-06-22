using System.Net;

namespace Managing.Domain.Exceptions;

public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);