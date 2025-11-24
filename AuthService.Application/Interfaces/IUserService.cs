using AuthService.Application.DTOs;


namespace AuthService.Application.Interfaces
{
    public  interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string? userId);

        Task UpdateProfileAsync(string? userId, UpdateProfileDto dto);

        Task<string> UploadAvatarAsync(string? userId, FileDto file);
    }
}
