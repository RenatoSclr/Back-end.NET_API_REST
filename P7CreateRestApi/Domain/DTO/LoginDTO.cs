using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}

