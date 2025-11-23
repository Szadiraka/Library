using AuthService.Application.DTOs;
using AuthService.Domain.Entities;


namespace AuthService.Application.Mapper
{
    public static class UserMapper
    {

        public static AppUser ToAppUser(RegisterDto user)
        {
            return new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = DateTime.UtcNow,

            };
        }

        public static UserDto ToUserDto(AppUser user, IList<string>? roles = null)
        {
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = user.CreatedAt,

            };
            if (roles != null)
            {
                userDto.Roles = roles.ToList();
            }
            return userDto;
        }

        public static UserAdminDto ToUserAdminDto(AppUser user, IList<string>? roles = null)
        {
            var userDto = new UserAdminDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailConfirmed = user.EmailConfirmed,
                IsBlocked = user.IsBlocked,
                BlockedAt = user.BlockedAt,
                BlockExpiresAt = user.BlockExpiresAt,
                BlockReason = user.BlockReason,
                LockoutEnd = user.LockoutEnd,
                IsDeleted = user.IsDeleted,
                DeletedAt = user.DeletedAt
            };

            if (roles != null)
            {
                userDto.Roles = roles.ToList();
            }
            return userDto;
        }


        public static BlockedUserDto ToBlockedUserDto(AppUser user, DateTimeOffset now)
        {
            var result = new BlockedUserDto
            {

                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LockoutDate = user.LockoutEnd,
                BlockExpiresAt = user.BlockExpiresAt,
                BlockReason = user.BlockReason,
                BlockType = user.LockoutEnd != null && user.LockoutEnd > now
                ? "System" : "Admin"

            };
            return result;
        }


    }
}
        
 

