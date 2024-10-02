namespace P7CreateRestApi.Domain.DTO.TradeDtos
{
    public class ReadTradeDTO
    {
        public int TradeId { get; set; }
        public double? BuyQuantity { get; set; }
        public double? SellQuantity { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public string? TradeSecurity { get; set; }
        public string? CreationName { get; set; }
        public string? RevisionName { get; set; }
        public string? Side { get; set; }
    }
}
