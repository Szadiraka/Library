using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Application.Services
{
   
    public class UserService: IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task<UserDto> GetUserByIdAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувача не авторизовано");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var roles = await _userManager.GetRolesAsync(user);

            return UserMapper.ToUserDto(user,roles);
        }
    }
}
