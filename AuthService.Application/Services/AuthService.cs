using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;



namespace AuthService.Application.Services
{
    public class AuthUserService : IAuthInterface
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthUserService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;

            _jwtTokenService = jwtTokenService;

        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingEmail != null)
            {
                throw new ConflictException($"Email '{registerDto.Email}' вже використовується");
            }

            var existingUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserName != null)
            {
                throw new ConflictException($"Username '{registerDto.UserName}' вже використовується");
            }

            var user = UserMapper.ToAppUser(registerDto);


            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new _ValidationException(string.Join("; ", result.Errors.Select(x => x.Description)));

            var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role.ToString());
            if (!roleResult.Succeeded)
                throw new BusinessRuleException($"Не вдалося додати роль '{registerDto.Role}'");

            return UserMapper.ToUserDto(user);

        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email) ??
                throw new UnauthorizedException("невірний логін або пароль");

            var validPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!validPassword)
                throw new UnauthorizedException("невірний логін або пароль");

            return await GenenerateAndSaveTokens(user);          

        }



        public async Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null)
                throw new UnauthorizedException("невалідний refresh токен");
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)

                throw new UnauthorizedException("refresh токен просрочении");


            return await GenenerateAndSaveTokens(user);          

        }



        private  async Task<AuthResponseDto> GenenerateAndSaveTokens(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenService.GenerateToken(user, roles);

            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtTokenService.RefreshTokenExpirationInDays());

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                throw new BusinessRuleException("не вдалося оновити дані");

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtTokenService.TokenExpirationInMinutes())
            };

        }
    }
}
