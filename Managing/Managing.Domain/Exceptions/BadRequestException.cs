using System.Net;

namespace Managing.Domain.Exceptions;

public class BadRequestException : RequestException
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) {}
}