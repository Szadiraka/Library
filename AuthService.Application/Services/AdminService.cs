using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mapper;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Exceptions;
using AuthService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AuthDbContext _context;

        public AdminService(UserManager<AppUser> userManager, AuthDbContext context, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
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
            var query = from u in _context.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId into userRoles
                        from ur in userRoles.DefaultIfEmpty()
                        join r in _context.Roles on ur.RoleId equals r.Id into roles
                        select new { User = u, Roles = roles };

            if (!string.IsNullOrEmpty(request.Search))
            {
                var s = request.Search.ToLower();

                query = query.Where(x => x.User.Email!.ToLower().Contains(s) ||
                    x.User.UserName!.ToLower().Contains(s) ||
                    (x.User.FirstName !=null && x.User.FirstName.ToLower().Contains(s)) ||
                    (x.User.LastName != null && x.User.LastName.ToLower().Contains(s))

                 );
            }
           

            if (request.IsBlocked != null)
            {
                query = query.Where(x => 
                     (x.User.IsBlocked || (x.User.LockoutEnd != null && x.User.LockoutEnd > DateTimeOffset.UtcNow))
                        == request.IsBlocked
                );
                     
            }

            if (request.EmailConfirmed != null)
                query = query.Where(u => u.User.EmailConfirmed == request.EmailConfirmed);

            if (request.OnlyDeleted == true)
                query = query.Where(u => u.User.IsDeleted);


            if (request.OnlyActive == true)
                query = query.Where(u => !u.User.IsDeleted);



            if (request.Roles != null && request.Roles.Any())
            {
                query = query.Where(x => x.Roles.Any(r => request.Roles.Contains(r.Name!)));
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderBy(x => x.User.Email)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var list = users.GroupBy(x => x.User)
                .Select(g =>
                {
                    var user = g.Key;
                    var roles = g.SelectMany(x => x.Roles).Select(r => r.Name).Distinct().ToList();
                    return UserMapper.ToUserAdminDto(user, roles!);
                });


            return new PagedResult<UserAdminDto>
            {
                Items = list?.ToList() ?? new List<UserAdminDto>(), 
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };

        }

       
        public async Task AddRoleToUserAsync(string userId, UserRoles role)
        {
           var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var roleExists = await _roleManager.RoleExistsAsync(role.ToString());

            if (!roleExists)
                throw new NotFoundException("роль не знайдено");

            var result =await  _userManager.AddToRoleAsync(user, role.ToString());

            if (!result.Succeeded)

                throw new BusinessRuleException(string.Join(", ", result.Errors.Select(x => x.Description)));


        }


        public async Task RemoveRoleFromUserAsync(string userId, UserRoles role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Користувача не знайдено");

            var roleExists = await _roleManager.RoleExistsAsync(role.ToString());

            if (!roleExists)
                throw new NotFoundException("роль не знайдено");

            var result = await _userManager.RemoveFromRoleAsync(user, role.ToString());

            if (!result.Succeeded)
                throw new BusinessRuleException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }
    }
}
