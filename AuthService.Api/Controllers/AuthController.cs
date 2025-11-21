using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authService;

        private readonly IUserContextService _contextService;
        public AuthController(IAuthInterface service, IUserContextService userContextService)
        {
            _authService = service;
            _contextService = userContextService;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });


            var user = await _authService.RegisterAsync(registerDto);
            return Ok(new ApiResponse { Message = "Реєстрація успішна", Data = user });


        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });
            var result = await _authService.LoginAsync(loginDto);
            return Ok(new ApiResponse { Message = "Успішний вхід", Data = result });



        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return Ok(new ApiResponse { Message = "токен успішно оновлено", Data = response });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = _contextService.UserId;

            await _authService.LogoutAsync(userId);

            return Ok(new ApiResponse { Message = "Ви успішно вийшли з акаунту", Data = null });
        }


        //-------------------------------------------------------
        [Authorize]
        [HttpPost("send-confirmation-email")]
        public async Task<IActionResult> SendEmailConfirmation()
        {
            var userId = _contextService.UserId;
            await _authService.SendEmailConfirmationAsync(userId);

            return Ok(new ApiResponse { Message = "Підтвердження відправлене на email" });
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequestDto request)
        {
            var result = await _authService.ConfirmEmailAsync(request.UserId, request.Token);

            return Ok(new ApiResponse { Message = result.Message });
        }

        //-------------------------------------------------------





        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new ApiResponse { Message = "Якщо email існує, на нього відправлено подальші інструкції" });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new ApiResponse { Message = "Ваш пароль було успішно змінено" });
        }



    }
}