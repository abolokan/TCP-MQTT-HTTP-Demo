using System.Net;
using Solid.Common.Exceptions;

namespace Solid.WebAPI.Common;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<CustomExceptionHandlerMiddleware> logger
        )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Error while request handling");

        HttpStatusCode code;

        switch (exception)
        {
            case ApiException ex:
                code = ex.StatusCode;
                break;

            case NoUpdateException _:
                code = HttpStatusCode.NotModified;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(exception.Message);
    }
}

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
