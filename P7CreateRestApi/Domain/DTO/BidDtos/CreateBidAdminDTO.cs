using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.BidDtos
{
    public class CreateBidAdminDTO
    {
        [Required]
        [StringLength(100)]
        public string? Account { get; set; }

        [Required]
        [StringLength(50)]
        public string? BidType { get; set; }

        [Required]
        [StringLength(50)]
        public string? Trader { get; set; }

        [Required]
        [StringLength(50)]
        public string? BidStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string? Book { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BidQuantity must be a positive number.")]
        public double? BidQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "AskQuantity must be a positive number.")]
        public double? AskQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BidValue must be a positive number.")]
        public double? BidValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ask must be a positive number.")]
        public double? Ask { get; set; }

        [Required]
        [StringLength(50)]
        public string? Benchmark { get; set; }

        [StringLength(50)]
        public string? CreationName { get; set; }

        [Required]
        [StringLength(50)]
        public string? BidSecurity { get; set; }

        [StringLength(500)]
        public string? Commentary { get; set; }

        [StringLength(100)]
        public string? DealName { get; set; }

        [StringLength(50)]
        public string? DealType { get; set; }

        [Required]
        [StringLength(50)]
        public string? Side { get; set; }
    }
}
