using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    };
        
 }

