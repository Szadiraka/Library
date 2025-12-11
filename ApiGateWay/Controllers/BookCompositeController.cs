using ApiGateWay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
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

            try
            {
                var token = Request.Headers["Authorization"].ToString();
                var _http = _httpClientFactory.CreateClient();

                _http.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

                var bookResponse = await _http.GetAsync(_options.BookService + $"/api/books/{id}");
                if (!bookResponse.IsSuccessStatusCode)
                    return NotFound(new ApiResponse<object> { Message = "Книга не знайдена" });


                string? resp = await bookResponse.Content.ReadAsStringAsync();
                ApiResponse<BookDto>? book = JsonSerializer.Deserialize<ApiResponse<BookDto>>(resp, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (book == null)
                    return BadRequest(new ApiResponse<object> { Message = "Не удалось десериализовать книгу" });

                //------------------------------------

                var requestBody = new {ids= book.Data!.AuthorIds};

                var authorResponse = await _http.PostAsync(_options.AuthorService + $"/api/author/get-by-ids",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

                if (!authorResponse.IsSuccessStatusCode)
                    return NotFound(new ApiResponse<object> { Message = "Автори не знайдені" });


                string? resp2 = await authorResponse.Content.ReadAsStringAsync();
                ApiResponse<List<AuthorDto>>? authors = JsonSerializer.Deserialize<ApiResponse<List<AuthorDto>>>(resp2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (authors == null)
                    return BadRequest(new ApiResponse<object> { Message = "Не удалось десериализовать авторів" });


                return Ok(new ApiResponse<object>
                {
                    Message = "дані сформовано",
                    Data = new
                    {
                        Book = book.Data,
                        Authors = authors.Data
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object> { Message=$"{ex.Message}"});
            }

           

        }

    }
}
