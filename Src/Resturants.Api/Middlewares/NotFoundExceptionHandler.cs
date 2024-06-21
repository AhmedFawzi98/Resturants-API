﻿using Microsoft.AspNetCore.Diagnostics;
using Resturants.Domain.Exceptions;

namespace Resturants.Api.Middlewares;

public class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = httpContext.Response;
        if (exception is not NotFoundException notFoundEx)
            return false;
        
        response.StatusCode = StatusCodes.Status404NotFound;
        response.ContentType = "text/plain";
        await response.WriteAsync(notFoundEx.Message).ConfigureAwait(false);
        _logger.LogError(notFoundEx,notFoundEx.Message);
        return true;        
    }
}
