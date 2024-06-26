using System.Net;

namespace Hotel.Shared.Exceptions;

public class RequestException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}