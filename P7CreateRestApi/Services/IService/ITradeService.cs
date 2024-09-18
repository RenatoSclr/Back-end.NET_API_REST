using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface ITradeService
    {
        Task<List<TradeDTO>> GetAllTradeDTOsAsync();

        Task CreateTradeAsync(TradeDTO tradeDTO);

        Task<Trade> GetTradeByIdAsync(int id);

        Task<TradeDTO> GetTradeDTOByIdAsync(int id);

        Task UpdateTradeAsync(TradeDTO tradeDTO, Trade trade);

        Task DeleteTradeAsync(int id);
    }
}
