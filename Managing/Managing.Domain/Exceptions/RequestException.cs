using System.Net;

namespace Managing.Domain.Exceptions;

public class RequestException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}