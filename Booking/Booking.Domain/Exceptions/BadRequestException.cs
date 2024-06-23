using System.Net;

namespace Booking.Domain.Exceptions;

public class BadRequestException(string message) : RequestException(HttpStatusCode.BadRequest, message);