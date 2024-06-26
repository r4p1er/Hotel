using Hotel.Shared.Exceptions;

namespace Booking.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception is RequestException
            ? (int)(exception as RequestException)!.StatusCode
            : StatusCodes.Status500InternalServerError;
        var result = new
        {
            message = context.Response.StatusCode == StatusCodes.Status500InternalServerError
                ? "Internal server error"
                : exception.Message
        };

        await context.Response.WriteAsJsonAsync(result);
    }
}