using System.Net;

namespace Identity.Domain.Exceptions;

public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);