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

        private readonly IBlobStorageService _blobStorageService;
        

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IBlobStorageService blobStorageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _blobStorageService = blobStorageService;
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

        public async Task UpdateProfileAsync(string? userId, UpdateProfileDto dto)
        {
           if(string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувача не авторизовано");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

             user.BirthDate = dto.BirthDate;
             user.FirstName = dto.FirstName;
             user.LastName = dto.LastName;
             user.UserName = dto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BusinessRuleException($"Не вдалося оновити дані користувача");

        }



        public async Task<string> UploadAvatarAsync(string? userId, FileDto fileDto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувача не авторизовано");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            string? url = await _blobStorageService.UploadFileAsync(fileDto, user.Id);

            if (url == null)
                throw new BusinessRuleException($"Не вдалося зберегти новий аватар");
            return url;
        }
    }
}
