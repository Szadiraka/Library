using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthService.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserContextService _userContextService;


        public UserController(IUserService userService, IUserContextService userContextService)
        {

            _userService = userService;
            _userContextService = userContextService;
        }


        [HttpGet("me")]      
        public async Task<IActionResult> GetMe()
        {
            var userId = _userContextService.UserId;
         
            var user = await _userService.GetUserByIdAsync(userId);

            return Ok(new ApiResponse { Message = "ok", Data = user });
        }


        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });


            var userId = _userContextService.UserId;

            await _userService.UpdateProfileAsync(userId, dto);

            return Ok(new ApiResponse { Message = "Дані профілю оновлено" });
        }


        [HttpPost("update-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse { Message = "Відсутній файл" });

            var userId = _userContextService.UserId;

            var fileDto = new FileDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = file.OpenReadStream()
            };
            

            var url = await _userService.UploadAvatarAsync(userId, fileDto);

            return Ok(new ApiResponse { Message = "Аватар оновлено", Data = url });
        }

    }
}
   