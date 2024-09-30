using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.BidDtos
{
    public class UpdateBidAdminDTO
    {
        public string Account { get; set; }
        public string BidType { get; set; }
        public string Trader { get; set; }
        public string BidStatus { get; set; }
        public string Book { get; set; } 
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? BidValue { get; set; }
        public double? Ask { get; set; }
        public string? RevisionName { get; set; }
        public string Benchmark { get; set; }
        public string BidSecurity { get; set; }
        public string Commentary { get; set; }
        public string DealName { get; set; } 
        public string DealType { get; set; }  
        public string Side { get; set; } 
    }
}
