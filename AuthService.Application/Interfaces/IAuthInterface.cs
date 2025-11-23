

using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IAuthInterface
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);

        Task LogoutAsync(string? userId);


        Task SendEmailConfirmationAsync(string? userId);

        Task<ConfirmEmailResponseDto> ConfirmEmailAsync(string userId, string token);


        Task ForgotPasswordAsync(string email);

        Task ResetPasswordAsync(ResetPasswordDto dto);

        Task ChangePasswordAsync (string? userId, ChangePasswordDto changePasswordDto);

        Task ChangeEmailRequestAsync(string? userId, string NewEmail);

        Task ConfirmEmailChangeAsync(string? userId,  ConfirmEmailChangeDto dto);

        Task DeleteAccountAsync(string? userId, string password);

       



    }
}
