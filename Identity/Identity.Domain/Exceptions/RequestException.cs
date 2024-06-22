using System.Net;

namespace Identity.Domain.Exceptions;

public class RequestException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}