using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthInterface _service) : ControllerBase
    {
      

        [HttpPost("register")]
       public async Task<IActionResult> Register ([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

          
                var user = await _service.RegisterAsync(registerDto);
                return Ok(new ApiResponse { Message = "Реєстрація успішна", Data = user });
            
            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });          
                var result = await _service.LoginAsync(loginDto);
                return Ok(new ApiResponse { Message = "Успішний вхід", Data = result });          
           
            
           
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await _service.RefreshTokenAsync(request.Token, request.RefreshToken);
            return Ok(new ApiResponse {Message = "токен успішно оновлено", Data = response });
        }
    }
}
