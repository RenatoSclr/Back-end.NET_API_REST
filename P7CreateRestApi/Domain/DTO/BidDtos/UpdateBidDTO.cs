using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.BidDtos
{
    public class UpdateBidDTO
    {
        [Range(0, double.MaxValue, ErrorMessage = "BidQuantity must be a positive number.")]
        public double? BidQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "AskQuantity must be a positive number.")]
        public double? AskQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BidValue must be a positive number.")]
        public double? BidValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ask must be a positive number.")]
        public double? Ask { get; set; }

        [StringLength(50)]
        public string? RevisionName { get; set; }

        [Required]
        [StringLength(50)]
        public string? BidSecurity { get; set; }

        [StringLength(500)]
        public string? Commentary { get; set; }

        [Required]
        [StringLength(50)]
        public string? Side { get; set; }  
    }
}
