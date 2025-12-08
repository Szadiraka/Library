using BooksService.Application.DTOs;
using BooksService.Application.Interfaces;
using BooksService.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService bookService)
        {
            _service = bookService;
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var result = await _service.GetBookByIdAsync(id);

            return Ok(new ApiResponse { Message = "Книку отримано", Data = result });
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var books = await _service.GetAllBooksAsync(query);
            return Ok(new ApiResponse { Message = "Книги отримано", Data = books });
        }


        [Authorize]
        [HttpGet("by-genre")]
        public async Task<IActionResult> GetAllBooksByGenreId([FromQuery] GenreIdQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var books = await _service.GetAllBooksByGenreIdAsync(query);
            return Ok(new ApiResponse { Message = "Книги отримано", Data = books });
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("deleted-books")]
        public async Task<IActionResult> GetAllDeletedBooks()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var books = await _service.GetAllDeletedBooksAsync();
            return Ok(new ApiResponse { Message = "Видалені книги отримано", Data = books });
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("restore-book/{id:guid}")]
        public async Task<IActionResult> RestoreBook(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var result = await _service.RestoreBookAsync(id);
            return Ok(new ApiResponse { Message = "Книгу відновлено", Data = result });
        }




        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBook([FromBody] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var book = await _service.AddBookAsync(dto);

            return Ok(new ApiResponse { Message = "Книгу створено", Data = book });


        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("update{id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var book = await _service.UpdateBookAsync(id, dto);

            return Ok(new ApiResponse { Message = "Дані про книгу оновлено", Data = book });
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("delete/{id:guid}")]

        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _service.DeleteBookAsync(id);
            return Ok(new ApiResponse { Message = "Книгу видалено" });
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("exists")]
        public async Task<IActionResult> BookExists([FromQuery] ExistBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var result = await _service.ExistsBookAsync(dto.Title, dto.PublisherYear);

            return Ok(new ApiResponse { Message = "запрос виконано вдало", Data = result });
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("by-author")]
        public async Task<IActionResult> GetAllBooksByAuthorId([FromQuery] BookAuthorQuery query)
        {
            var result = await _service.GetAllBooksByAuthorIdAsync(query);
            return Ok(new ApiResponse { Message = "Книги отримано", Data = result });
        }


       

    }
}
