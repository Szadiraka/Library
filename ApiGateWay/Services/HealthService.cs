using ApiGateWay.Models;
using Microsoft.Extensions.Options;

namespace ApiGateWay.Services
{
    public class HealthService : IHealthService
    {

        private readonly AddressesOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public HealthService(IOptions<AddressesOptions> options, IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<ApiResponse<object>> GetAllCheckHealth()
        {
            var _http = _httpClientFactory.CreateClient();
        
            var authService = SafeCall( _http, _options.AuthService+ "/health");
            var authorService = SafeCall(_http, _options.AuthorService+ "/health");
            var bookService = SafeCall( _http, _options.BookService+ "/health");
            var emailService =SafeCall( _http, _options.EmailService+ "/health");


            await Task.WhenAll(authService, authorService,  bookService, emailService);
         
            var result = new ApiResponse<object>()
            {
                Message= "Результати отримано",
                Data = new Dictionary<string, string>()
                {
                    ["gateWay"] = "Healthy",
                    ["authService"] = await authService,
                    ["authorService"] = await authorService,
                    ["bookService"] = await  bookService,
                    ["emailService"] = await  emailService
                }
            };
            return result;
                
        }


        private async Task<string> SafeCall(HttpClient _http, string url)
        {
            try
            {
                var response = await _http.GetAsync(url);
                     return response.IsSuccessStatusCode ? "Healthy" : "UnHealthy";
            }
            catch
            {
                return "UnHealthy";
            }
        }

           

            

        
    }
}
