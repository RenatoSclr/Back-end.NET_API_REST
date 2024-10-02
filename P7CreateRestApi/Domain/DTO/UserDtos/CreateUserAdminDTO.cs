using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.UserDtos
{
    public class CreateUserAdminDTO
    {
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "FullName cannot exceed 100 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        public IList<string>? Roles { get; set; }
    }
}

