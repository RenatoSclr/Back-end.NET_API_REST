using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO.BidDtos;

namespace Dot.Net.WebApi.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<List<ReadBidAdminDTO>> GetAllBidDTOsAsAdminAsync()
        {
            var bidList = await _bidRepository.GetAllAsync();
            return MapToBidAdminDTOList(bidList.ToList());
        }

        public async Task<List<ReadBidDTO>> GetAllBidDTOsAsUserAsync()
        {
            var bidList = await _bidRepository.GetAllAsync();
            return MapToBidDTOList(bidList.ToList());
        }

        public async Task CreateBidAsUserAsync(CreateBidDTO bidDTO)
        {
            var bid = MapCreateBidDTOToBid(bidDTO);
            await _bidRepository.AddAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task CreateBidAsAdminAsync(CreateBidAdminDTO bidAdminDTO)
        {
            var bid = MapCreateBidAdminDTOToBid(bidAdminDTO);
            await _bidRepository.AddAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task UpdateBidAsUserAsync(UpdateBidDTO bidDTO, Bid existingBid)
        {
            var bid = MapUpdateBidDTOToBid(bidDTO, existingBid);
            await _bidRepository.UpdateAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task UpdateBidAsAdminAsync(UpdateBidAdminDTO bidAdminDTO, Bid existingBid)
        {
            var bid = MapUpdateBidAdminDTOToBid(bidAdminDTO, existingBid);
            await _bidRepository.UpdateAsync(bid);
            await _bidRepository.SaveAsync();
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _bidRepository.GetByIdAsync(id);
        }


        public async Task<ReadBidDTO> GetBidDTOByIdAsync(int id)
        {
            var bid = await GetBidByIdAsync(id);
            return MapToBidDTO(bid);
        }

        public async Task<ReadBidAdminDTO> GetBidAdminDTOByIdAsync(int id)
        {
            var bid = await GetBidByIdAsync(id);
            return MapToBidAdminDTO(bid);
        }

        public async Task DeleteBidAsync(int id)
        {
            await _bidRepository.DeleteAsync(id);
            await _bidRepository.SaveAsync();
        }


        private Bid MapCreateBidDTOToBid(CreateBidDTO bidDTO)
        {
            var bid = new Bid();
            bid.BidQuantity = bidDTO.BidQuantity;
            bid.AskQuantity = bidDTO.AskQuantity;
            bid.Ask = bidDTO.Ask;
            bid.BidSecurity = bidDTO.BidSecurity;
            bid.BidValue = bidDTO.BidValue;
            bid.Commentary = bidDTO.Commentary;
            bid.CreationDate = DateTime.Now;
            bid.CreationName = bidDTO.CreationName;
            bid.Side = bidDTO.Side;
            return bid;
        }

        private Bid MapUpdateBidDTOToBid(UpdateBidDTO bidDTO, Bid existingBid)
        {
            var bid = existingBid;
            bid.BidQuantity = bidDTO.BidQuantity;
            bid.AskQuantity = bidDTO.AskQuantity;
            bid.Ask = bidDTO.Ask;
            bid.BidSecurity = bidDTO.BidSecurity;
            bid.BidValue = bidDTO.BidValue;
            bid.Commentary = bidDTO.Commentary;
            bid.RevisionName = bidDTO.RevisionName;
            bid.RevisionDate = DateTime.Now;
            bid.Side = bidDTO.Side;
            return bid;
        }


        private Bid MapCreateBidAdminDTOToBid(CreateBidAdminDTO bidDTO)
        {
            var bid = new Bid();
            bid.Account = bidDTO.Account;
            bid.BidType = bidDTO.BidType;
            bid.Trader = bidDTO.Trader;
            bid.BidStatus = bidDTO.BidStatus;
            bid.Book = bidDTO.Book;
            bid.BidQuantity = bidDTO.BidQuantity;
            bid.AskQuantity = bidDTO.AskQuantity;
            bid.Ask = bidDTO.Ask;
            bid.Benchmark = bidDTO.Benchmark;
            bid.BidSecurity = bidDTO.BidSecurity;
            bid.BidValue = bidDTO.BidValue;
            bid.Commentary = bidDTO.Commentary;
            bid.DealName = bidDTO.DealName;
            bid.DealType = bidDTO.DealType;
            bid.CreationName = bidDTO.CreationName;
            bid.CreationDate = DateTime.Now;
            bid.Side = bidDTO.Side;
            return bid;
        }

        private Bid MapUpdateBidAdminDTOToBid(UpdateBidAdminDTO bidDTO, Bid existingBid)
        {
            var bid = existingBid;
            bid.Account = bidDTO.Account;
            bid.BidType = bidDTO.BidType;
            bid.Trader = bidDTO.Trader;
            bid.BidStatus = bidDTO.BidStatus;
            bid.Book = bidDTO.Book;
            bid.BidQuantity = bidDTO.BidQuantity;
            bid.AskQuantity = bidDTO.AskQuantity;
            bid.Ask = bidDTO.Ask;
            bid.Benchmark = bidDTO.Benchmark;
            bid.BidSecurity = bidDTO.BidSecurity;
            bid.BidValue = bidDTO.BidValue;
            bid.Commentary = bidDTO.Commentary;
            bid.DealName = bidDTO.DealName;
            bid.DealType = bidDTO.DealType;
            bid.RevisionName = bidDTO.RevisionName;     
            bid.RevisionDate = DateTime.Now;
            bid.Side = bidDTO.Side;
            return bid;
        }


        private ReadBidAdminDTO MapToBidAdminDTO(Bid bid)
        {
            return new ReadBidAdminDTO
            {
                BidDTOId = bid.BidId,
                Account = bid.Account,
                BidType = bid.BidType,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                BidValue = bid.BidValue,
                Ask = bid.Ask,
                Benchmark = bid.Benchmark,
                BidListDate = bid.BidListDate,
                BidStatus = bid.BidStatus,
                BidSecurity = bid.BidSecurity,
                Trader = bid.Trader,
                Book = bid.Book,
                Commentary = bid.Commentary,
                CreationDate = bid.CreationDate,
                CreationName = bid.CreationName,
                RevisionDate = bid.RevisionDate,
                RevisionName = bid.RevisionName,
                DealName = bid.DealName,
                DealType = bid.DealType,
                Side = bid.Side
            };
        }
        private ReadBidDTO MapToBidDTO(Bid bid)
        {
            return new ReadBidDTO
            {
                BidId = bid.BidId,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                BidValue = bid.BidValue,
                Ask = bid.Ask,
                RevisionName = bid.RevisionName,
                CreationName = bid.CreationName,
                BidSecurity = bid.BidSecurity,
                Commentary = bid.Commentary,
                Side = bid.Side
            };
        }

        private List<ReadBidAdminDTO> MapToBidAdminDTOList(List<Bid> bidList)
        {
            return bidList.Select(bid => new ReadBidAdminDTO
            {
                BidDTOId = bid.BidId,
                Account = bid.Account,
                BidType = bid.BidType,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                BidValue = bid.BidValue,
                Ask = bid.Ask,
                Benchmark = bid.Benchmark,
                BidListDate = bid.BidListDate,
                BidStatus = bid.BidStatus,
                BidSecurity = bid.BidSecurity,
                Trader = bid.Trader,
                Book = bid.Book,
                Commentary = bid.Commentary,
                CreationDate = bid.CreationDate,
                CreationName = bid.CreationName,
                RevisionDate = bid.RevisionDate,
                RevisionName = bid.RevisionName,
                DealName = bid.DealName,
                DealType = bid.DealType,
                Side = bid.Side
            }).ToList();
        }

        private List<ReadBidDTO> MapToBidDTOList(List<Bid> bidList)
        {
            return bidList.Select(bid => new ReadBidDTO
            {
                BidId = bid.BidId,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                BidValue = bid.BidValue,
                Ask = bid.Ask,
                CreationName= bid.CreationName,
                RevisionName= bid.RevisionName,
                BidSecurity = bid.BidSecurity,
                Commentary = bid.Commentary,
                Side = bid.Side
            }).ToList();
        }
    }
}
