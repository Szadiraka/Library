using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Microsoft.AspNetCore.Identity;


namespace AuthService.Application.Services
{
    public class AuthUserService : IAuthInterface
    {
        private readonly UserManager<AppUser> _userManager;      
        private readonly IJwtTokenService _jwtTokenService;

        public AuthUserService( UserManager<AppUser> userManager,IJwtTokenService jwtTokenService)
        {
             _userManager = userManager;
           
             _jwtTokenService = jwtTokenService;
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingEmail = await  _userManager.FindByEmailAsync(registerDto.Email);
            if (existingEmail != null)
            {
                throw new Exception("Email вже використовується");
            }

            var existingUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserName != null)
            {
                throw new Exception("Ім'я користувача вже використовується");
            }

             var user = UserMapper.ToAppUser(registerDto);


             var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(x => x.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
            if(!roleResult.Succeeded)
                throw new Exception("Не вдалось додати ролі");

            return UserMapper.ToUserDto(user);
            
        }


        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ??          
                throw new Exception("Користувача не знайдено");

            var validPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
             if(!validPassword)
                throw new Exception("Пароль не валідний");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenService.GenerateToken(user,roles);

            return token;

        }
    }
}
