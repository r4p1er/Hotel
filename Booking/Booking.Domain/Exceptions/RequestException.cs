using System.Net;

namespace Booking.Domain.Exceptions;

public class RequestException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode = statusCode;
}