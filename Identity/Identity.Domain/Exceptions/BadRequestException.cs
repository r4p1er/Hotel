using System.Net;

namespace Identity.Domain.Exceptions;

public class BadRequestException : RequestException
{
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) {}
}