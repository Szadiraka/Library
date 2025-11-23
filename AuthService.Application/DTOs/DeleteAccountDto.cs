using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class DeleteAccountDto
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
