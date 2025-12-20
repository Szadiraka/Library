using Blob.Application.Interfaces;
using System.Security.Claims;

namespace Blob.Api.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContex;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContex = httpContextAccessor;
        }

        public string? UserId => _httpContex.HttpContext?.User?
                                 .FindFirst(ClaimTypes.NameIdentifier)?.Value;

    }
}
