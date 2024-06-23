using System.Net;

namespace Booking.Domain.Exceptions;

public class NotFoundException(string message) : RequestException(HttpStatusCode.NotFound, message);