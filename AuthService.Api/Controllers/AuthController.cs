using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
          
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var response = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return Ok(new ApiResponse { Message = "токен успішно оновлено", Data = response });
        }


        [Authorize]
        [HttpPost("logout")]
   
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
   
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequestDto request)
        {
        
            var result = await _authService.ConfirmEmailAsync(request.UserId, request.Token);

            return Ok(new ApiResponse { Message = result.Message });
        }

        //-------------------------------------------------------

    
        [HttpPost("forgot-password")]     
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });


            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new ApiResponse { Message = "Якщо email існує, на нього відправлено подальші інструкції" });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            await _authService.ResetPasswordAsync(dto);
            return Ok(new ApiResponse { Message = "Ваш пароль було успішно змінено" });
        }

        //------------------------------------------------------------------
      

        [Authorize]
        [HttpPut("change-password")]  
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var userId = _contextService.UserId;
            await _authService.ChangePasswordAsync(userId, dto);
            return Ok(new ApiResponse { Message = "Ваш пароль було успішно змінено" });
        }

    

        [Authorize]
        [HttpPost("change-email-request")]
        public async Task<IActionResult> ChangeEmailRequest([FromBody]  ChangeEmailRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var userId = _contextService.UserId;

            await _authService.ChangeEmailRequestAsync(userId, dto.NewEmail);
            return Ok(new ApiResponse { Message = "Підтвердження відправлене на email" });
        }

        [Authorize]
        [HttpPost("confirm-email-change")] 
        public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var userId = _contextService.UserId;

            await _authService.ConfirmEmailChangeAsync(userId, dto);

            return Ok(new ApiResponse { Message = "Електронная адреса змінена" });
        }


        //------------------------------------------------------

        [Authorize]
        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse { Message = "Не коректний запит" });

            var userId = _contextService.UserId;
            await _authService.DeleteAccountAsync(userId, dto.Password);

            return Ok(new ApiResponse { Message = "Ваш аккаунт був деактивований" });
        }


      
       
     

    }
}