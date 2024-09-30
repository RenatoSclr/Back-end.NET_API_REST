using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.TradeDtos;

namespace Dot.Net.WebApi.Services.IService
{
    public interface ITradeService
    {
        Task<List<ReadTradeAdminDTO>> GetAllTradeDTOsAsAdminAsync();
        Task<List<ReadTradeDTO>> GetAllTradeDTOsAsUserAsync();

        Task CreateTradeAsAdminAsync(CreateTradeAdminDTO tradeDTO);
        Task CreateTradeAsUserAsync(CreateTradeDTO tradeDTO);

        Task<Trade> GetTradeByIdAsync(int id);

        Task<ReadTradeAdminDTO> GetTradeDTOAsAdminByIdAsync(int id);
        Task<ReadTradeDTO> GetTradeDTOAsUserByIdAsync(int id);

        Task UpdateTradeAsAdminAsync(UpdateTradeAdminDTO tradeDTO, Trade trade);
        Task UpdateTradeAsUserAsync(UpdateTradeDTO tradeDTO, Trade trade);

        Task DeleteTradeAsync(int id);
    }
}
