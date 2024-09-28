using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.UserDtos
{
    public class CreateUserAdminDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? FullName { get; set; }
        [Required]
        public string Password { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
