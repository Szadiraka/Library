

using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IAuthInterface
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
    }
}
