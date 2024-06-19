using System.Net;

namespace Identity.Domain.Exceptions;

public class NotFoundException : RequestException
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) {}
}