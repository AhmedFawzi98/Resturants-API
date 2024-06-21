using System.Diagnostics;

namespace Resturants.Api.Middlewares
{
    public class RequestTimeLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeLoggingMiddleware> _logger;
        public RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await next.Invoke(context);
            sw.Stop();
            if(sw.ElapsedMilliseconds > 4000)
                _logger.LogInformation($"[{context.Request.Method}]request at path:{context.Request.Path} took {sw.ElapsedMilliseconds} ms");
        }
    }
}
