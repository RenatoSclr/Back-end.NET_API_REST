using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.RuleNameDtos
{
    public class EditRuleNameAdminDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Json content is required.")]
        public string? Json { get; set; }

        [Required(ErrorMessage = "Template is required.")]
        public string? Template { get; set; }

        [Required(ErrorMessage = "SQL string is required.")]
        public string? SqlStr { get; set; }

        [StringLength(1000, ErrorMessage = "SQL part cannot exceed 1000 characters.")]
        public string? SqlPart { get; set; }
    }
}

