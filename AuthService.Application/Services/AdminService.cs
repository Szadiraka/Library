using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task RestoreAccountAsync(string email)
        {
            var user = await _userManager.FindByIdAsync(email);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");
            if (!user.IsDeleted)
                throw new BusinessRuleException($"Користувач активний");

            user.IsDeleted = false;
            user.DeletedAt = null;

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BusinessRuleException("не вдалося відновити аккаунт");
        }


        // блокування користувача
        public async Task BlockUserAsync(BlockUserDto dto)
        {
            var user =await _userManager.FindByIdAsync(dto.UserId);

            if(user == null)
                throw new NotFoundException("Користувача не знайдено");

            if(user.IsBlocked)
                throw new ConflictException("Користувач вже заблокований");

            user.IsBlocked = true;
            user.BlockReason = dto.Reason;
            user.BlockedAt = DateTime.UtcNow;
            user.BlockExpiresAt = dto.ExpiresAt;

            await _userManager.UpdateAsync(user);

        }


        //розблокування користувача
        public async Task UnblockUserAsync(UnblockUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");
            if (!user.IsBlocked)
                throw new ConflictException("Користувач не блокований");

            user.IsBlocked = false;
            user.BlockedAt = null;
            user.BlockReason = null;
            user.BlockExpiresAt = null;

            await _userManager.UpdateAsync(user);
        }

        // отримання блокованих користувач
        public async Task<List<BlockedUserDto>> GetBlockedUserAsync()
        {

            var now = DateTimeOffset.UtcNow;

            var users = _userManager.Users.Where(x => x.IsDeleted == false)
                      .Where(x => (x.LockoutEnd != null && x.LockoutEnd > now) ||

                  (x.IsBlocked && x.BlockExpiresAt!= null && x.BlockExpiresAt>now.UtcDateTime ) ||

                  (x.IsBlocked  && x.BlockExpiresAt == null));

            var list = await users.Select(user => UserMapper.ToBlockedUserDto(user, now))
                       .ToListAsync();

            return list;
        }

        // отримання даних про користувача по id

        public async Task<UserAdminDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("користувача не знайдено");

            var roles = await _userManager.GetRolesAsync(user);

            var result = UserMapper.ToUserAdminDto(user, roles);

            return result;

        }


        public async Task<PagedResult<UserAdminDto>> GetUsersAsync(UserFilterRequest request)
        {
            var query = _userManager.Users.AsQueryable();

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.ToLower();

                query = query.Where(x => x.Email!.ToLower().Contains(s) ||
                    x.UserName!.ToLower().Contains(s) ||
                    (x.FirstName !=null && x.FirstName.ToLower().Contains(s)) ||
                    (x.LastName != null && x.LastName.ToLower().Contains(s))

                 );
            }

            if (request.IsDeleted != null)
            {
                query = query.Where(x => x.IsDeleted == request.IsDeleted);
            }

            if (request.IsBlocked != null)
            {
                query = query.Where(x=>x.IsBlocked == request.IsBlocked);
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderBy(x => x.LastName)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            //var result = users.Select(user => {
               
            //    UserMapper.ToUserAdminDto(user,await _userManager.GetRolesAsync(user))
            //    }).ToList();
            var list = new List<UserAdminDto>();
            foreach (var user in users) {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = UserMapper.ToUserAdminDto(user, roles);
                list.Add(userDto);
            }
            //  добавить фильтр оп ролее!!!!!!!!!!!!!!!!!!!!!!

            return new PagedResult<UserAdminDto>
            {
                Items = list,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };

        }






    }
}
