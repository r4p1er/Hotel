using System.Net;

namespace Managing.Domain.Exceptions;

public class BadRequestException(string message) : RequestException(HttpStatusCode.BadRequest, message);