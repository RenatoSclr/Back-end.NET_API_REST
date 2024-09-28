using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.UserDtos
{
    public class ReadUserDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
