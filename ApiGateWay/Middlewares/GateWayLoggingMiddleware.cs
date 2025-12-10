using System.Diagnostics;

namespace ApiGateWay.Middlewares
{
    public class GateWayLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GateWayLoggingMiddleware> _logger;


        public GateWayLoggingMiddleware(RequestDelegate next, ILogger<GateWayLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var cid = context.Items["X-Correlation-Id"]?.ToString();
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("GateWay request:  {Method} {Path} started, CID: {cid}", context.Request.Method, context.Request.Path, cid);

            await _next(context);

            sw.Stop();

            _logger.LogInformation("GateWay response: {Method} {Path} finished with status {StatusCode} in {Elapsed} ms, CID: {cid}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds, cid);
        }
    }
}
