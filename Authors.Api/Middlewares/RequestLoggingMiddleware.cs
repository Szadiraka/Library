using System.Diagnostics;

namespace Authors.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;


        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var cid = context.Items["X-Correlation-Id"]?.ToString();

            var sw = Stopwatch.StartNew();
            _logger.LogInformation("HTTP {Method} {Path} started, CID: {cid}", context.Request.Method, context.Request.Path, cid);

            await _next(context);

            sw.Stop();

            _logger.LogInformation("HTTP {Method} {Path} finished with status {StatusCode} in {Elapsed} ms, CID: {cid}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds, cid);
        }

    }
}
