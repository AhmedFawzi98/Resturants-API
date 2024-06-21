using Microsoft.AspNetCore.Diagnostics;
using Resturants.Domain.Exceptions;

namespace Resturants.Api.Middlewares;

public class ForbiddenExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ForbiddenExceptionHandler> _logger;

    public ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = httpContext.Response;
        if (exception is not ForbiddenException forbiddenEx)
            return false;
        
        response.StatusCode = StatusCodes.Status403Forbidden;
        response.ContentType = "text/plain";
        await response.WriteAsync(forbiddenEx.Message).ConfigureAwait(false);
        _logger.LogError(forbiddenEx, forbiddenEx.Message);
        return true;        
    }
}
