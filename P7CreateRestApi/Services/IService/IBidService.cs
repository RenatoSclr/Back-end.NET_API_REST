using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IBidService
    {
        List<BidDTO> GetAllBidDTOs();

        void CreateBid(BidDTO bidDTO);

        Bid GetBidById(int id);

        BidDTO GetBidDTOById(int id);

        void UpdateBid(BidDTO bidDTO, Bid bid);

        void DeleteBid(int id);
    }
}
