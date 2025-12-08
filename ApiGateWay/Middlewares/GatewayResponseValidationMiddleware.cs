using ApiGateWay.Models;
using System.Net;
using System.Text.Json;

namespace ApiGateWay.Middlewares
{
    public class GatewayResponseValidationMiddleware
    {

        public readonly RequestDelegate _next;

        public GatewayResponseValidationMiddleware(RequestDelegate _delegate)
        {
            _next = _delegate;
        }

        public  async Task InvokeAsync(HttpContext context)
        {

            var originalBody = context.Response.Body;

            using var newStream = new MemoryStream();
            context.Response.Body = newStream;

            try
            {
                await _next(context);           

                newStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(newStream).ReadToEndAsync();
                var statusCode = context.Response.StatusCode;

                if (IsApiResponse(responseBody))
                {
                    context.Response.Body = originalBody;
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(responseBody);
                    return;
                }

                var message = GetMessageByStatusCode(statusCode);

                var wrappedResponse = new ApiResponse() { Message = message, Data = null };
                var json = JsonSerializer.Serialize(wrappedResponse);

                context.Response.Body = originalBody;
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);

            }
            catch(Exception ex)
            {
                context.Response.Body = originalBody;
                string message = $"Внутрішня помилка шлюзу: {ex.Message}";              

                var wrappedResponse = new ApiResponse() { Message = message, Data = null };
                var json = JsonSerializer.Serialize(wrappedResponse);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);

            }





            
        }

        private static bool IsApiResponse( string? response)
        {
            if(string.IsNullOrWhiteSpace(response)) return false;

            try
            {
                var obj = JsonSerializer.Deserialize<ApiResponse>(response);
                return obj != null;
            }
            catch
            {
                return false;
            }

        }

        private string GetMessageByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Помилка валідації",
                401 => "Не авторизований. Токен не валідний або не переданий",
                403 => "Заборонено. Ви не маєте доступу",
                404 => "Ресурс не знайдено",
                409 => "Конфлікт",
                422 => "Сутність, що не піддається обробці",
                500 => "Внутрішня помилка сервера",
                502 => "Неправильний шлюз. Мікросервіс недоступний",
                503 => "Сервіс недоступний",
                504 => "Час очікування шлюзу перевищено",
                _ => "Неочікувана помилка"
            };
        }


    }
}
