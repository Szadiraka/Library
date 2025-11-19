using AuthService.Application.DTOs;


namespace AuthService.Application.Interfaces
{
    public  interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string? userId);
    }
}
