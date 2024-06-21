using Microsoft.AspNetCore.Diagnostics;

namespace Resturants.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = httpContext.Response;

        _logger.LogError(exception, exception.Message);
        response.StatusCode = StatusCodes.Status500InternalServerError;
        response.ContentType = "text/plain";
        await response.WriteAsync($"An unexpected internal server error occurred., exception message {exception.Message}").ConfigureAwait(false);
        return true;
    }
}
