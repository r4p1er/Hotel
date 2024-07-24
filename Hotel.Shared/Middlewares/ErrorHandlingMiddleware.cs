using System.Text.Json;
using Hotel.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Hotel.Shared.Middlewares;

/// <summary>
/// Middleware для централизованной обработки исключений
/// </summary>
/// <param name="next"></param>
public class ErrorHandlingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Вызвать middleware
    /// </summary>
    /// <param name="context"><inheritdoc cref="HttpContext"/></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    /// <summary>
    /// Обработать исключение
    /// </summary>
    /// <param name="context"><inheritdoc cref="HttpContext"/></param>
    /// <param name="exception">Вызванное исключение</param>
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

        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}