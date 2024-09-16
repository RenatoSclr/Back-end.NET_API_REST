using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services
{
    public class BidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository) 
        {
            _bidRepository = bidRepository;
        }

        public List<BidDTO> GetAllBidDTOs()
        {
            var bidList = _bidRepository.GetAll().ToList();
            return MapToBidDTOList(bidList);
        }

        public void CreateBid(BidDTO bidDTO)
        {
            var bid = MapToBid(bidDTO);
            _bidRepository.Add(bid);
            _bidRepository.Save();
        }

        public Bid MapToBid(BidDTO bidDTO)
        {
            var bid = new Bid();
            bid.Account = bidDTO.Account;
            bid.BidType = bidDTO.BidDTOType;
            bid.BidQuantity = bidDTO.BidDTOQuantity;

            return bid;
        }

        public Bid GetBidById(int id)
        {
            return _bidRepository.GetById(id);   
        }

        public BidDTO GetBidDTOById(int id)
        {
            var bid = _bidRepository.GetById(id);
            return MapToBidDTO(bid);
        }


        public void UpdateBid(BidDTO bidDTO)
        {
            Bid bid = MapToBid(bidDTO);
            _bidRepository.Update(bid);
            _bidRepository.Save();

        }

        public void DeleteBid(int id)
        {
            _bidRepository.Delete(id);
            _bidRepository.Save();
        }

        public BidDTO MapToBidDTO(Bid bid) 
        {
            BidDTO bidDTO = new BidDTO();

            bidDTO.BidDTOId = bid.BidId;
            bidDTO.Account = bid.Account;
            bidDTO.BidDTOType = bid.BidType;
            bidDTO.BidDTOQuantity = bid.BidQuantity;

            return bidDTO;
        }

        public List<BidDTO> MapToBidDTOList(List<Bid> bidList)
        {
            var bidDTOlist = new List<BidDTO>();
            foreach (var bid in bidList) 
            {
                BidDTO bidDTO = new BidDTO();

                bidDTO.BidDTOId = bid.BidId;
                bidDTO.Account = bid.Account;
                bidDTO.BidDTOType = bid.BidType;
                bidDTO.BidDTOQuantity = bid.BidQuantity;

                bidDTOlist.Add(bidDTO);
            }
            
            return bidDTOlist;
        }


    }
}
