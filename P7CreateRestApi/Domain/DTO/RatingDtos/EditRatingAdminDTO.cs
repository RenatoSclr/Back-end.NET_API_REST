using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.RatingDtos
{
    public class EditRatingAdminDTO
    {
        [Required(ErrorMessage = "MoodysRating is required.")]
        [StringLength(50, ErrorMessage = "MoodysRating cannot exceed 50 characters.")]
        public string? MoodysRating { get; set; }

        [Required(ErrorMessage = "SandPRating is required.")]
        [StringLength(50, ErrorMessage = "SandPRating cannot exceed 50 characters.")]
        public string? SandPRating { get; set; }

        [Required(ErrorMessage = "FitchRating is required.")]
        [StringLength(50, ErrorMessage = "FitchRating cannot exceed 50 characters.")]
        public string? FitchRating { get; set; }

        [Required(ErrorMessage = "OrderNumber is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderNumber must be a positive integer.")]
        public int? OrderNumber { get; set; }
    }
}
