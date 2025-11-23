using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

      
        [HttpPost("restore-account")]
        public async Task<IActionResult> RestoreAccount([FromBody] RestoreAccountRequestDto dto)
        {
            await _adminService.RestoreAccountAsync(dto.Email);

            return Ok(new ApiResponse { Message = "Aккаунт відновлено" });
        }



        [HttpPost("block-user")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserDto dto)
        {
            await _adminService.BlockUserAsync(dto);
            return Ok(new ApiResponse { Message = "Користувача заблоковано" });
        }


        [HttpPost("unblock-user")]
        public async Task<IActionResult> UnBlockUser([FromBody] UnblockUserDto dto)
        {
            await _adminService.UnblockUserAsync(dto);
            return Ok(new ApiResponse { Message = "Користувача разблоковано" });
        }


        [HttpGet("users-blocked")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            var users = await _adminService.GetBlockedUserAsync();

            return Ok(new ApiResponse { Message = "Список заблокованих користувачів", Data = users });
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _adminService.GetUserByIdAsync(userId);
            return Ok(new ApiResponse { Message="інформація про користувача отримана", Data=result});
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromBody] UserFilterRequest request)
        {
            var result = await _adminService.GetUsersAsync(request);
            return Ok(new ApiResponse {Message="інформація про користувачів отримана", Data= result});
        }


    }
}
