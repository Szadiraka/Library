using BooksService.Application.DTOs;
using BooksService.Application.Interfaces;
using BooksService.Application.Mappers;
using BooksService.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorService _service;

        public BookAuthorController(IBookAuthorService bookAuthorService)
        {
            _service = bookAuthorService;
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBookAuthor([FromBody] BookAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var result = await _service.AddBookAuthorAsync(dto);

            return Ok(new ApiResponse { Message = "Книга-автор створено", Data = result });

        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteBookAuthor([FromBody] BookAuthorDto dto)
        {
            await _service.DeleteBookAuthorAsync(dto);
            return Ok(new ApiResponse { Message = "Книга-автор видалено" });
        }


        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("authors_id/{bookId:guid}")]
        public async Task<IActionResult> GetAuthorsIdsByBookId(Guid bookId)
        {
            var result = await _service.GetAuthorsIdsByBookIdAsync(bookId);
            return Ok(new ApiResponse { Message = "Авторів отримано", Data = result });
        }

    }
}
