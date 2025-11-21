using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace AuthService.Api.Controllers
{


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
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userId = _userContextService.UserId;
         
            var user = await _userService.GetUserByIdAsync(userId);

            return Ok(new ApiResponse { Message = "ok", Data = user });
        }
    }
}
   