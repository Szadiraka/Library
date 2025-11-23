using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class BlockedUserDto
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTimeOffset? LockoutDate { get; set; }

        public DateTime? BlockExpiresAt { get; set; }  

        public string? BlockReason { get; set; } = string.Empty;
        
        public string BlockType { get; set; } = string.Empty; // ким блоковано

       
    }
}
