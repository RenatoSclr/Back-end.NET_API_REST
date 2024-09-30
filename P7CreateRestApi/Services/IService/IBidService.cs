using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.BidDtos;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IBidService
    {
        Task<List<ReadBidAdminDTO>> GetAllBidDTOsAsAdminAsync();
        Task<List<ReadBidDTO>> GetAllBidDTOsAsUserAsync();

        Task CreateBidAsUserAsync(CreateBidDTO bidDTO);

        Task CreateBidAsAdminAsync(CreateBidAdminDTO bidAdminDTO);

        Task<Bid> GetBidByIdAsync(int id);

        Task<ReadBidDTO> GetBidDTOByIdAsync(int id);

        Task<ReadBidAdminDTO> GetBidAdminDTOByIdAsync(int id);

        Task UpdateBidAsUserAsync(UpdateBidDTO bidDTO, Bid bid);

        Task UpdateBidAsAdminAsync(UpdateBidAdminDTO bidDTO, Bid bid);

        Task DeleteBidAsync(int id);
    }
}
