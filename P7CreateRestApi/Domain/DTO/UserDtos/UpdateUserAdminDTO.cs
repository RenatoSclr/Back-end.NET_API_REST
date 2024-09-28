using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.UserDtos
{
    public class UpdateUserAdminDTO
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? FullName { get; set; }
        [Required]
        public IList<string>? Roles { get; set; }
    }
}
