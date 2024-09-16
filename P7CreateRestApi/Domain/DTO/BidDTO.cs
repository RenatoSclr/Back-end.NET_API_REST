namespace P7CreateRestApi.Domain.DTO
{
    public class BidDTO
    {
        public int BidDTOId { get; set; }
        public string Account { get; set; }
        public string BidDTOType { get; set; }
        public double? BidDTOQuantity { get; set; }
    }
}
