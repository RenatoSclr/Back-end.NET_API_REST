using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO
{
    public class CreateUserDTO
    {
        public UserDTO UserDTO { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
