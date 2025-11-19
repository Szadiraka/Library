using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user, IList<string> roles);

        string GenerateRefreshToken();

        public int TokenExpiration { get; }

        public int RefreshTokenExpiration { get; }


    }
}
