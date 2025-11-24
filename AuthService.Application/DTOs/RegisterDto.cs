using AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public  class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;


        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;


        [Range(typeof(DateTime), "1900-01-01", "2025-01-01", ErrorMessage ="Invalid birth date")]
        public DateTime? BirthDate { get; set; }



        public string? AvatarUrl { get; set; }       
   


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRoles Role { get; set; } = UserRoles.User;
    }
}
