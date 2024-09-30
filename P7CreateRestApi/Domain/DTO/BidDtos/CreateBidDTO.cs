using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.BidDtos
{
    public class CreateBidDTO
    {
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? BidValue { get; set; }
        public double? Ask { get; set; }
        public string BidSecurity { get; set; }
        public string? CreationName { get; set; }
        public string Commentary { get; set; }
        public string Side { get; set; }  
    }
}
