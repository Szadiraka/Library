using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;



namespace AuthService.Application.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailNotificationService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService,
            IConfiguration configuration, IEmailNotificationService emailService)
        {
            _userManager = userManager;

            _jwtTokenService = jwtTokenService;
            
            _emailService = emailService;
            _configuration = configuration;
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

            if (user.IsDeleted)
                throw new ForbiddenException("аккаунт видалено");

            if (user.IsBlocked)
            {
                if (user.BlockExpiresAt == null || user.BlockExpiresAt > DateTime.UtcNow)
                    throw new ForbiddenException("Ваш аккаунт заблоковано: " + user.BlockReason);


                user.BlockExpiresAt = null;
                user.IsBlocked = false;
                user.BlockReason = null;
                user.BlockedAt = null;

                await _userManager.UpdateAsync(user);
            }




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

            if(user.IsDeleted)
                throw new ForbiddenException("аккаунт видалено");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedException("refresh токен просрочений");

            return await GenenerateAndSaveTokens(user);          

        }



        private  async Task<AuthResponseDto> GenenerateAndSaveTokens(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenService.GenerateToken(user, roles);

            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtTokenService.RefreshTokenExpiration);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                throw new BusinessRuleException("не вдалося оновити дані");

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtTokenService.TokenExpiration)
            };

        }


        public async Task LogoutAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user =await  _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = null;

            var result =await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BusinessRuleException("Не вдалось збросити refresh токен");
        }

        //---------------

        public async Task SendEmailConfirmationAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) 
                throw new NotFoundException("Користувача не знайдено");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //повертаємо на почту користувача посилання для підтвердження
            var confirmationLink = $"{_configuration["Client:Url_Server"]}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

         
            await _emailService.SendEmailAsync(user,EmailTemplate.EmailConfirmation, confirmationLink);
        }


        public async Task<ConfirmEmailResponseDto> ConfirmEmailAsync(string userId, string token)
        {
           var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");
         
                var decodedToken = WebEncoders.Base64UrlDecode(token);

                var normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ConfirmEmailAsync(user, normalToken);

          

            if (!result.Succeeded)
            {
              
                await _emailService.SendEmailAsync(user,EmailTemplate.ConfirmedEmail,"Не вдалося підтвердити email. Токен недійсний або прострочений.");
                throw new BusinessRuleException("Недійсний або просрочений токен");
            }
            string msg = "Вашу почту підтверджено";
            await _emailService.SendEmailAsync(user,EmailTemplate.ConfirmedEmail, msg);
            return new ConfirmEmailResponseDto(){ Message=msg};
           

        }

        //---------------

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //повертаємо  на UI- посилання для генерації нового пароля
            var resetLink = $"{_configuration["Client:Url_Client"]}/reset-password?userId={user.Id}&token={encodedToken}";

          
            await _emailService.SendEmailAsync(user, EmailTemplate.PasswordForgot, resetLink);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
           var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var decodeToken = WebEncoders.Base64UrlDecode(dto.Token);
            var normalToken = Encoding.UTF8.GetString(decodeToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                throw new BusinessRuleException("Не вдалося скинути пароль");
            }        
            await _emailService.SendEmailAsync(user,EmailTemplate.ResetPassword, "Вітаємо дорогий клієнт! Ваш пароль успішно змінено.");
               


        }


        public async Task ChangePasswordAsync(string? userId, ChangePasswordDto dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user =await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new BusinessRuleException($"Не вдалося змінити пароль: {errors}");
            }
                
        }



        public async Task ChangeEmailRequestAsync(string? userId, string newEmail)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user,newEmail);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
          
            await _emailService.SendEmailAsync(user, EmailTemplate.ChangeEmail, encodedToken);
        }


        public async Task ConfirmEmailChangeAsync(string? userId, ConfirmEmailChangeDto dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");


            var decodedToken = WebEncoders.Base64UrlDecode(dto.Token);

            var normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ChangeEmailAsync(user, dto.NewEmail, normalToken);

            if (!result.Succeeded)
            {
               throw new BusinessRuleException("Невдалося оновити почту");
            }
          
           
        }

        
        public async Task DeleteAccountAsync(string? userId, string password)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Користувач не авторизований");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if(!isPasswordValid)
                throw new UnauthorizedException("невірний пароль");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            user.RefreshToken=string.Empty;
            user.RefreshTokenExpiryTime = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BusinessRuleException("не вдалося видалити аккаунт");

            
        }

       
    }
}
