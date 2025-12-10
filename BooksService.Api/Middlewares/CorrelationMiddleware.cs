namespace BooksService.Api.Middlewares
{
    public class CorrelationMiddleware
    {

        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Correlation-Id", out var cid)
                && !string.IsNullOrEmpty(cid))
            {
                context.Items["X-Correlation-Id"] = cid.ToString();
            }

            await _next(context);
        }
    }
}
