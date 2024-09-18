namespace P7CreateRestApi.Domain.DTO
{
    public class BidDTO
    {
        public int BidDTOId { get; set; }
        public string Account { get; set; }
        public string BidDTOType { get; set; }
        public double? BidDTOQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? BidDTOValue { get; set; }
        public double? Ask { get; set; }
        public string Benchmark { get; set; }
        public DateTime? BidDTOListDate { get; set; }
        public string Commentary { get; set; }
        public string BidDTOSecurity { get; set; }
        public string BidDTOStatus { get; set; }
        public string Trader { get; set; }
        public string Book { get; set; }
        public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string SourceListId { get; set; }
        public string Side { get; set; }
    }
}
