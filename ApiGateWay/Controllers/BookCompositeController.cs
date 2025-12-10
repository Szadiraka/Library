using ApiGateWay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ApiGateWay.Controllers
{
    [Route("gateway/books")]
    [ApiController]
    public class BookCompositeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AddressesOptions _options;

        public BookCompositeController(IHttpClientFactory httpClientFactory, IOptions<AddressesOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;

        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookWithAuthors(Guid id)
        {
            var token = Request.Headers["Authorization"].ToString();
            var _http = _httpClientFactory.CreateClient();

            _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            var bookResponse = await _http.GetAsync(_options.BookService + $"/api/books/{id}");
            if(!bookResponse.IsSuccessStatusCode)
                 return NotFound(new ApiResponse<object> {Message = "Книга не знайдена"});


            string? resp = await bookResponse.Content.ReadAsStringAsync();
            ApiResponse<BookDto>? response = JsonSerializer.Deserialize <ApiResponse<BookDto>>( resp, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // нужно попробовать изменить ответ, чтобы включал = id - авторов, чтобы не делать три запроса


            // вторым этапом нужно получить авторов ( коллекция по коллекции id)  и добавить в ответ


            return Ok(new ApiResponse<object> {Message = response?.Message, Data = response?.Data });

        }

    }
}
