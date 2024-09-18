using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<List<BidDTO>> GetAllBidDTOsAsync()
        {
            var bidList = await _bidRepository.GetAllAsync();
            return MapToBidDTOList(bidList.ToList());
        }

        public async Task CreateBidAsync(BidDTO bidDTO)
        {
            var bid = MapToBid(bidDTO);
            await _bidRepository.AddAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task UpdateBidAsync(BidDTO bidDTO, Bid existingBid)
        {
            var bid = MapToBid(bidDTO, existingBid);
            await _bidRepository.UpdateAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _bidRepository.GetByIdAsync(id);
        }


        public async Task<BidDTO> GetBidDTOByIdAsync(int id)
        {
            var bid = await _bidRepository.GetByIdAsync(id);
            return MapToBidDTO(bid);
        }

        public async Task DeleteBidAsync(int id)
        {
            await _bidRepository.DeleteAsync(id);
            await _bidRepository.SaveAsync();
        }


        private Bid MapToBid(BidDTO bidDTO, Bid existingBid = null)
        {
            var bid = existingBid ?? new Bid();
            bid.Account = bidDTO.Account;
            bid.BidType = bidDTO.BidDTOType;
            bid.BidQuantity = bidDTO.BidDTOQuantity;
            bid.AskQuantity = bidDTO.AskQuantity;
            bid.Ask = bidDTO.Ask;
            bid.BidListDate = bidDTO.BidDTOListDate;
            bid.BidStatus = bidDTO.BidDTOStatus;
            bid.BidSecurity = bidDTO.BidDTOSecurity;
            bid.BidValue = bidDTO.BidDTOValue;
            bid.Benchmark = bidDTO.Benchmark;
            bid.Trader = bidDTO.Trader;
            bid.Book = bidDTO.Book;
            bid.Commentary = bidDTO.Commentary;
            bid.CreationDate = bidDTO.CreationDate;
            bid.CreationName = bidDTO.CreationName;
            bid.RevisionDate = bidDTO.RevisionDate;
            bid.RevisionName = bidDTO.RevisionName;
            bid.DealName = bidDTO.DealName;
            bid.DealType = bidDTO.DealType;
            bid.SourceListId = bidDTO.SourceListId;
            bid.Side = bidDTO.Side;

            return bid;
        }

        private BidDTO MapToBidDTO(Bid bid)
        {
            return new BidDTO
            {
                BidDTOId = bid.BidId,
                Account = bid.Account,
                BidDTOType = bid.BidType,
                BidDTOQuantity = bid.BidQuantity,
                Ask = bid.Ask,
                BidDTOListDate = bid.BidListDate,
                BidDTOStatus = bid.BidStatus,
                BidDTOSecurity = bid.BidSecurity,
                BidDTOValue = bid.BidValue,
                Benchmark = bid.Benchmark,
                Trader = bid.Trader,
                Book = bid.Book,
                Commentary = bid.Commentary,
                CreationDate = bid.CreationDate,
                CreationName = bid.CreationName,
                RevisionDate = bid.RevisionDate,
                RevisionName = bid.RevisionName,
                DealName = bid.DealName,
                DealType = bid.DealType,
                SourceListId = bid.SourceListId,
                Side = bid.Side
            };
        }

        private List<BidDTO> MapToBidDTOList(List<Bid> bidList)
        {
            return bidList.Select(bid => new BidDTO
            {
                BidDTOId = bid.BidId,
                Account = bid.Account,
                BidDTOType = bid.BidType,
                BidDTOQuantity = bid.BidQuantity,
                Ask = bid.Ask,
                BidDTOListDate = bid.BidListDate,
                BidDTOStatus = bid.BidStatus,
                BidDTOSecurity = bid.BidSecurity,
                BidDTOValue = bid.BidValue,
                Benchmark = bid.Benchmark,
                Trader = bid.Trader,
                Book = bid.Book,
                Commentary = bid.Commentary,
                CreationDate = bid.CreationDate,
                CreationName = bid.CreationName,
                RevisionDate = bid.RevisionDate,
                RevisionName = bid.RevisionName,
                DealName = bid.DealName,
                DealType = bid.DealType,
                SourceListId = bid.SourceListId,
                Side = bid.Side
            }).ToList();
        }
    }
}
