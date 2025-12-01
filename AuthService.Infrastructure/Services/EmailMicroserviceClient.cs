using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using System.Net.Http.Headers;

using System.Text;
using System.Text.Json;

namespace AuthService.Infrastructure.Services
{
    public class EmailMicroserviceClient : IEmailMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        public EmailMicroserviceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailRequestAsync(EmailRequest request, string apiToken)
        {      

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "api/email/send")
            {
                Content = new StringContent(JsonSerializer.Serialize(request),Encoding.UTF8,"application/json" )
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer ", apiToken);

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ConflictException($"Не вдалось відправити повідомлення. ПомилкаЖ {error}");
            }
             

        }
    }
}
