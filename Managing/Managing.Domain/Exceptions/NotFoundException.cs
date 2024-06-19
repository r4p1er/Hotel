using System.Net;

namespace Managing.Domain.Exceptions;

public class NotFoundException : RequestException
{
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) {}
}