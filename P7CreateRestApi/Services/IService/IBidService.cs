using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IBidService
    {
        List<BidDTO> GetAllBidDTOs();

        void CreateBid(Bid bid);

        Bid GetBidById(int id);

        BidDTO GetBidDTOById(int id);

        void UpdateBid(Bid bid);

        void DeleteBid(int id);

        BidDTO MapToBidDTO(Bid bid);

        List<BidDTO> MapToBidDTOList(List<Bid> bidList);

        Bid MapTOBid(BidDTO bidDTO);
    }
}
