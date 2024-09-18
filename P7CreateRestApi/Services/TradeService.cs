using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<List<TradeDTO>> GetAllTradeDTOsAsync()
        {
            var tradeList = await _tradeRepository.GetAllAsync();
            return MapToTradeDTOList(tradeList.ToList());
        }

        public async Task CreateTradeAsync(TradeDTO tradeDTO)
        {
            var trade = MapToTrade(tradeDTO);
            await _tradeRepository.AddAsync(trade);
            await _tradeRepository.SaveAsync();
        }

        public async Task UpdateTradeAsync(TradeDTO tradeDTO, Trade existingTrade)
        {
            var trade = MapToTrade(tradeDTO, existingTrade);
            await _tradeRepository.UpdateAsync(trade);
            await _tradeRepository.SaveAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.GetByIdAsync(id);
        }


        public async Task<TradeDTO> GetTradeDTOByIdAsync(int id)
        {
            var trade = await _tradeRepository.GetByIdAsync(id);
            return MapToTradeDTO(trade);
        }

        public async Task DeleteTradeAsync(int id)
        {
            await _tradeRepository.DeleteAsync(id);
            await _tradeRepository.SaveAsync();
        }


        private Trade MapToTrade(TradeDTO tradeDTO, Trade existingTrade = null)
        {
            var trade = existingTrade ?? new Trade();
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
            trade.CreationDate = tradeDTO.CreationDate;
            trade.RevisionName = tradeDTO.RevisionName;
            trade.RevisionDate = tradeDTO.RevisionDate;
            trade.DealName = tradeDTO.DealName;
            trade.DealType = tradeDTO.DealType;
            trade.SourceListId = tradeDTO.SourceListId;
            trade.Side = tradeDTO.Side; 

            return trade;
        }

        private TradeDTO MapToTradeDTO(Trade trade)
        {
            return new TradeDTO
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
                SourceListId = trade.SourceListId,
                Side = trade.Side
        };
        }

        private List<TradeDTO> MapToTradeDTOList(List<Trade> tradeList)
        {
            return tradeList.Select(trade => new TradeDTO
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
                SourceListId = trade.SourceListId,
                Side = trade.Side
            }).ToList();
        }
    }
}
