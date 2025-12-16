using Blob.Application.Dtos;
using Blob.Application.Exceptions;
using Minio.Exceptions;
using System.Net;
using System.Text.Json;

namespace Blob.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var cid = context.Items["X-Correlation-Id"]?.ToString();

                _logger.LogWarning(ex,"Помилка для запиту {Method} {Path}. CID: {cid}", context.Request.Method, context.Request.Path,  cid);

                var statusCode = MapExceptionToStatusCode(ex);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Відповідь вже розпочата, використання відповіді не можливе. CID: {cid}", cid);
                    throw;
                }

                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new ApiResponse<object> { Message = GetClientMessage(ex) };

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);


            }
        }



        private static int MapExceptionToStatusCode(Exception ex)
        {

            var result = ex switch
            {

                NotFoundException _ => HttpStatusCode.NotFound,
                _ValidationException _ => HttpStatusCode.BadRequest,
                ConflictException _ => HttpStatusCode.Conflict,
                UnauthorizedException _ => HttpStatusCode.Unauthorized,
                Application.Exceptions.ForbiddenException _ => HttpStatusCode.Forbidden,
                BusinessRuleException _ => HttpStatusCode.UnprocessableEntity,

                MinioException _ => HttpStatusCode.InternalServerError,

                _ => HttpStatusCode.InternalServerError,


            };
            return (int)result;

        }


        private static string GetClientMessage(Exception ex)
        {

            return ex switch
            {

                NotFoundException vex => vex.Message,
                ConflictException vex => vex.Message,
                _ValidationException vex => vex.Message,
                Application.Exceptions.ForbiddenException vex => vex.Message,
                UnauthorizedException vex => vex.Message,
                BusinessRuleException vex => vex.Message,

                MinioException vex => "Сховище файлів тимчасово недоступне",

                _ => "Сталася неочікувана помилка"

            };
        }
    }
}
