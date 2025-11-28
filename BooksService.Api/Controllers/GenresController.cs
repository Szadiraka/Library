using BooksService.Application.DTOs;
using BooksService.Application.Interfaces;
using BooksService.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenresController(IGenreService genreService)
        {
            _service = genreService;
        }

        //[Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGenreById(Guid id)
        {
            var result = await _service.GetGenreByIdAsync(id);

            return Ok(new ApiResponse { Message = "Жанр отримано", Data = result });
        }


        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllGenres([FromQuery] GenreQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var genres = await _service.GetAllGenresAsync(query);
            return Ok(new ApiResponse { Message = "Жанри отримано", Data = genres });
        }


        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet("deleted-genres")]
        public async Task<IActionResult> GetAllDeletedGenres()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var genres = await _service.GetAllDeletedGenresAsync();
            return Ok(new ApiResponse { Message = "Видалені жанри отримано", Data = genres });
        }

        //[Authorize(Roles = "Admin,Librarian")]
        [HttpPut("restore-genre/{id:guid}")]
        public async Task<IActionResult> RestoreGenre(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var result = await _service.RestoreGenreAsync(id);
            return Ok(new ApiResponse { Message = "Жанр відновлено", Data = result });
        }




        //[Authorize(Roles = "Admin,Librarian")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var genre = await _service.AddGenreAsync(dto);

            return Ok(new ApiResponse { Message = "Жанр створено", Data = genre });


        }


        //[Authorize(Roles = "Admin,Librarian")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var genre = await _service.UpdateGenreAsync(id, dto);

            return Ok(new ApiResponse { Message = "Дані про жанр оновлено", Data = genre });
        }


        //[Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("{id:guid}")]

        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            await _service.DeleteGenreAsync(id);
            return Ok(new ApiResponse { Message = "Жанр видалено" });
        }


        //[Authorize(Roles = "Admin,Librarian")]
        [HttpGet("exists")]
        public async Task<IActionResult> GenreExists([FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var result = await _service.ExistsGenreAsync(name);

            return Ok(new ApiResponse { Message = "запрос виконано вдало", Data = result });
        }

    }
}
