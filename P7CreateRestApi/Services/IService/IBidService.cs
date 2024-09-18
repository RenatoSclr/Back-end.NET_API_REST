using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IBidService
    {
        Task<List<BidDTO>> GetAllBidDTOsAsync();

        Task CreateBidAsync(BidDTO bidDTO);

        Task<Bid> GetBidByIdAsync(int id);

        Task<BidDTO> GetBidDTOByIdAsync(int id);

        Task UpdateBidAsync(BidDTO bidDTO, Bid bid);

        Task DeleteBidAsync(int id);
    }
}
