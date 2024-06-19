using System.Net;

namespace Identity.Domain.Exceptions;

public class RequestException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    
    public RequestException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}