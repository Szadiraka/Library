

using AuthService.Application.DTOs;

namespace AuthService.Application.Interfaces
{
    public interface IAuthInterface
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        Task<string> LoginAsync(LoginDto loginDto);
    }
}
