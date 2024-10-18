using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.TradeDtos
{
    public class UpdateTradeDTO
    {
        [Range(0, double.MaxValue, ErrorMessage = "BuyQuantity must be a positive number.")]
        public double? BuyQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SellQuantity must be a positive number.")]
        public double? SellQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BuyPrice must be a positive number.")]
        public double? BuyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SellPrice must be a positive number.")]
        public double? SellPrice { get; set; }

        [Required(ErrorMessage = "TradeSecurity is required.")]
        [StringLength(50, ErrorMessage = "TradeSecurity cannot exceed 50 characters.")]
        public string? TradeSecurity { get; set; }

        [StringLength(50, ErrorMessage = "RevisionName cannot exceed 50 characters.")]
        public string? RevisionName { get; set; }

        [Required(ErrorMessage = "Side is required.")]
        [StringLength(50, ErrorMessage = "Side cannot exceed 50 characters.")]
        public string? Side { get; set; }
    }
}
