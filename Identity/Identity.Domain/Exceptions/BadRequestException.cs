using System.Net;

namespace Identity.Domain.Exceptions;

public class BadRequestException(string message) : RequestException(HttpStatusCode.BadRequest, message);