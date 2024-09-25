using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO
{
    public class UserDTO
    {
        public string Id { get; set; } 
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; } 
        public string? FullName { get; set; } 
    }
}
