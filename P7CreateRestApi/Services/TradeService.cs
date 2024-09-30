using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO.TradeDtos;

namespace Dot.Net.WebApi.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<List<ReadTradeAdminDTO>> GetAllTradeDTOsAsAdminAsync()
        {
            var tradeList = await _tradeRepository.GetAllAsync();
            return MapToTradeAdminDTOList(tradeList.ToList());
        }

        public async Task<List<ReadTradeDTO>> GetAllTradeDTOsAsUserAsync()
        {
            var tradeList = await _tradeRepository.GetAllAsync();
            return MapToTradeUserDTOList(tradeList.ToList());
        }

        public async Task CreateTradeAsAdminAsync(CreateTradeAdminDTO tradeDTO)
        {
            var trade = MapCreateTradeDtoAdminToTrade(tradeDTO);
            await _tradeRepository.AddAsync(trade);
            await _tradeRepository.SaveAsync();
        }
        public async Task CreateTradeAsUserAsync(CreateTradeDTO tradeDTO)
        {
            var trade = MapCreateTradeDtoUserToTrade(tradeDTO);
            await _tradeRepository.AddAsync(trade);
            await _tradeRepository.SaveAsync();
        }

        public async Task UpdateTradeAsAdminAsync(UpdateTradeAdminDTO tradeDTO, Trade existingTrade)
        {
            var trade = MapUpdateTradeDtoAdminToTrade(tradeDTO, existingTrade);
            await _tradeRepository.UpdateAsync(trade);
            await _tradeRepository.SaveAsync();
        }

        public async Task UpdateTradeAsUserAsync(UpdateTradeDTO tradeDTO, Trade existingTrade)
        {
            var trade = MapUpdateTradeDtoUserToTrade(tradeDTO, existingTrade);
            await _tradeRepository.UpdateAsync(trade);
            await _tradeRepository.SaveAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.GetByIdAsync(id);
        }


        public async Task<ReadTradeAdminDTO> GetTradeDTOAsAdminByIdAsync(int id)
        {
            var trade = await GetTradeByIdAsync(id);
            return MapToTradeAdminDTO(trade);
        }

        public async Task<ReadTradeDTO> GetTradeDTOAsUserByIdAsync(int id)
        {
            var trade = await GetTradeByIdAsync(id);
            return MapToTradeUserDTO(trade);
        }

        public async Task DeleteTradeAsync(int id)
        {
            await _tradeRepository.DeleteAsync(id);
            await _tradeRepository.SaveAsync();
        }


        private Trade MapCreateTradeDtoAdminToTrade(CreateTradeAdminDTO tradeDTO)
        {
            var trade = new Trade();
            trade.Account = tradeDTO.Account;
            trade.AccountType = tradeDTO.AccountType;
            trade.BuyQuantity = tradeDTO.BuyQuantity;
            trade.SellQuantity = tradeDTO.SellQuantity;
            trade.BuyPrice = tradeDTO.BuyPrice;
            trade.SellPrice = tradeDTO.SellPrice;
            trade.TradeDate = tradeDTO.TradeDate;
            trade.TradeSecurity = tradeDTO.TradeSecurity;
            trade.TradeStatus = tradeDTO.TradeStatus;
            trade.Trader = tradeDTO.Trader;
            trade.Benchmark = tradeDTO.Benchmark;
            trade.Book = tradeDTO.Book;
            trade.CreationName = tradeDTO.CreationName;
            trade.CreationDate = DateTime.Now;
            trade.DealName = tradeDTO.DealName;
            trade.DealType = tradeDTO.DealType;
            trade.Side = tradeDTO.Side; 

            return trade;
        }


        private Trade MapUpdateTradeDtoAdminToTrade(UpdateTradeAdminDTO tradeDTO, Trade existingTrade)
        {
            var trade = existingTrade;
            trade.Account = tradeDTO.Account;
            trade.AccountType = tradeDTO.AccountType;
            trade.BuyQuantity = tradeDTO.BuyQuantity;
            trade.SellQuantity = tradeDTO.SellQuantity;
            trade.BuyPrice = tradeDTO.BuyPrice;
            trade.SellPrice = tradeDTO.SellPrice;
            trade.TradeDate = tradeDTO.TradeDate;
            trade.TradeSecurity = tradeDTO.TradeSecurity;
            trade.TradeStatus = tradeDTO.TradeStatus;
            trade.Trader = tradeDTO.Trader;
            trade.Benchmark = tradeDTO.Benchmark;
            trade.Book = tradeDTO.Book;
            trade.RevisionDate = DateTime.Now;
            trade.RevisionName = tradeDTO.RevisionName;
            trade.DealName = tradeDTO.DealName;
            trade.DealType = tradeDTO.DealType;
            trade.Side = tradeDTO.Side;

            return trade;
        }
        private Trade MapCreateTradeDtoUserToTrade(CreateTradeDTO tradeDTO)
        {
            var trade = new Trade();
            trade.BuyQuantity = tradeDTO.BuyQuantity;
            trade.SellQuantity = tradeDTO.SellQuantity;
            trade.BuyPrice = tradeDTO.BuyPrice;
            trade.SellPrice = tradeDTO.SellPrice;
            trade.TradeSecurity = tradeDTO.TradeSecurity;
            trade.CreationName = tradeDTO.CreationName;
            trade.CreationDate = DateTime.Now;
            trade.Side = tradeDTO.Side;

            return trade;
        }

        private Trade MapUpdateTradeDtoUserToTrade(UpdateTradeDTO tradeDTO, Trade existingTrade)
        {
            var trade = existingTrade;
            trade.BuyQuantity = tradeDTO.BuyQuantity;
            trade.SellQuantity = tradeDTO.SellQuantity;
            trade.BuyPrice = tradeDTO.BuyPrice;
            trade.SellPrice = tradeDTO.SellPrice;
            trade.TradeSecurity = tradeDTO.TradeSecurity;
            trade.RevisionDate = DateTime.Now;
            trade.RevisionName = tradeDTO.RevisionName;
            trade.Side = tradeDTO.Side;

            return trade;
        }

        private ReadTradeAdminDTO MapToTradeAdminDTO(Trade trade)
        {
            return new ReadTradeAdminDTO
            {
                TradeId = trade.TradeId,
                Account = trade.Account,
                AccountType = trade.AccountType,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationName = trade.CreationName,
                CreationDate = trade.CreationDate,
                RevisionName = trade.RevisionName,
                RevisionDate = trade.RevisionDate,
                DealName = trade.DealName,
                DealType = trade.DealType,
                Side = trade.Side
            };
        }

        private ReadTradeDTO MapToTradeUserDTO(Trade trade)
        {
            return new ReadTradeDTO
            {
                TradeId = trade.TradeId,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeSecurity = trade.TradeSecurity,
                CreationName = trade.CreationName,
                RevisionName = trade.RevisionName,
                Side = trade.Side
            };
        }

        private List<ReadTradeDTO> MapToTradeUserDTOList(List<Trade> tradeList)
        {
            return tradeList.Select(trade => new ReadTradeDTO
            {
                TradeId = trade.TradeId,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeSecurity = trade.TradeSecurity,
                CreationName = trade.CreationName,
                RevisionName = trade.RevisionName,
                Side = trade.Side
            }).ToList();
        }

        private List<ReadTradeAdminDTO> MapToTradeAdminDTOList(List<Trade> tradeList)
        {
            return tradeList.Select(trade => new ReadTradeAdminDTO
            {
                TradeId = trade.TradeId,
                Account = trade.Account,
                AccountType = trade.AccountType,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationName = trade.CreationName,
                CreationDate = trade.CreationDate,
                RevisionName = trade.RevisionName,
                RevisionDate = trade.RevisionDate,
                DealName = trade.DealName,
                DealType = trade.DealType,
                Side = trade.Side
            }).ToList();
        }
    }
}
