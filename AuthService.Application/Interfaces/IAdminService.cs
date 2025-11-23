using AuthService.Application.DTOs;


namespace AuthService.Application.Interfaces
{
    public interface IAdminService
    {
        Task RestoreAccountAsync(string email);

        Task BlockUserAsync(BlockUserDto dto);

        Task UnblockUserAsync(UnblockUserDto dto);

        Task<List<BlockedUserDto>> GetBlockedUserAsync();

        Task<UserAdminDto> GetUserByIdAsync(string userId);

        Task<PagedResult<UserAdminDto>> GetUsersAsync(UserFilterRequest filter);
    }
}
