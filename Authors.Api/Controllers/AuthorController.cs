using Authors.Application.DTOs;
using Authors.Application.Interfaces;
using Authors.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
namespace Authors.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService authorService)
        {
            _service = authorService;
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await _service.GetByIdAsync(id);

            return Ok(new ApiResponse { Message = "Автора отримано", Data = author });
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery] AuthorQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var authors = await _service.GetAllAsync(query);
            return Ok(new ApiResponse { Message = "Авторів отримано", Data = authors });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var author = await _service.AddAsync(dto);

            return Ok(new ApiResponse { Message = "Автора створено", Data = author });


        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var author = await _service.UpdateAsync(id, dto);

            return Ok(new ApiResponse { Message = "Дані про автора оновлено", Data = author });
        }

        [HttpDelete("{id:guid}")]

        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponse { Message = "Автора видалено" });
        }

        [HttpGet("exists")]
        public async Task<IActionResult> AuthorExists([FromQuery] ExistAuthorDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

           var result =  await _service.ExistsAsync(dto.FullName, dto.BirthDate);

            return Ok(new ApiResponse { Message = "запрос виконано вдало", Data = result });
        }


    }
}
