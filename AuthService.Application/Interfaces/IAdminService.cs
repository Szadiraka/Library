using AuthService.Application.DTOs;
using AuthService.Domain.Enums;


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

        Task AddRoleToUserAsync(string userId, UserRoles role);

        Task  RemoveRoleFromUserAsync(string userId, UserRoles role);
    }
}
