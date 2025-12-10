namespace ApiGateWay.Middlewares
{
    public class CorrelationIdMiddleware
    {

        private readonly RequestDelegate _next;
        private const string HeaderName = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = string.Empty;

            if(context.Request.Headers.TryGetValue(HeaderName, out var cid) &&
                !string.IsNullOrWhiteSpace(cid))
            {
                correlationId = cid.ToString();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[HeaderName] = correlationId;
            }

            context.Items[HeaderName] = correlationId;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = correlationId;
                return Task.CompletedTask;
            });

            await _next(context);
        }


    }
}
