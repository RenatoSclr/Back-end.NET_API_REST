using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services;
using Moq;
using P7CreateRestApi.Domain.DTO.TradeDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class TradeServiceTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly TradeService _tradeService;

        public TradeServiceTests()
        {
            _tradeRepositoryMock = new Mock<ITradeRepository>();
            _tradeService = new TradeService(_tradeRepositoryMock.Object);
        }

        private List<Trade> GetListTradeEntity()
        {
            return new List<Trade>
            {
                new Trade
                {
                    TradeId = 1,
                    Account = "Account 1",
                    AccountType = "Type 1",
                    BuyQuantity = 100.0,
                    SellQuantity = 50.0,
                    BuyPrice = 1000.0,
                    SellPrice = 950.0,
                    TradeDate = DateTime.Now,
                    TradeSecurity = "Security 1",
                    TradeStatus = "Active",
                    Trader = "Trader 1",
                    Benchmark = "Benchmark 1",
                    Book = "Book 1",
                    CreationName = "System",
                    CreationDate = DateTime.Now.AddDays(-10),
                    RevisionName = "Revision 1",
                    RevisionDate = DateTime.Now,
                    DealName = "Deal 1",
                    DealType = "Type A",
                    Side = "Buy"
                },
                new Trade
                {
                    TradeId = 2,
                    Account = "Account 2",
                    AccountType = "Type 2",
                    BuyQuantity = 200.0,
                    SellQuantity = 100.0,
                    BuyPrice = 2000.0,
                    SellPrice = 1950.0,
                    TradeDate = DateTime.Now,
                    TradeSecurity = "Security 2",
                    TradeStatus = "Inactive",
                    Trader = "Trader 2",
                    Benchmark = "Benchmark 2",
                    Book = "Book 2",
                    CreationName = "System",
                    CreationDate = DateTime.Now.AddDays(-20),
                    RevisionName = "Revision 2",
                    RevisionDate = DateTime.Now,
                    DealName = "Deal 2",
                    DealType = "Type B",
                    Side = "Sell"
                }
            };
        }

        [Fact]
        public async Task GetAllTradeDTOsAsAdminAsync_ShouldReturnTradeDTOList_WhenTradesExist()
        {
            // Arrange
            _tradeRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListTradeEntity());

            // Act
            var result = await _tradeService.GetAllTradeDTOsAsAdminAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Account 1", result[0].Account);
            Assert.Equal("Account 2", result[1].Account);
        }

        [Fact]
        public async Task GetAllTradeDTOsAsUserAsync_ShouldReturnTradeDTOList_WhenTradesExist()
        {
            // Arrange
            _tradeRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListTradeEntity());

            // Act
            var result = await _tradeService.GetAllTradeDTOsAsUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Security 1", result[0].TradeSecurity);
            Assert.Equal("Security 2", result[1].TradeSecurity);
        }

        [Fact]
        public async Task CreateTradeAsAdminAsync_ShouldAddAndSaveTrade()
        {
            // Arrange
            var createTradeAdminDTO = new CreateTradeAdminDTO
            {
                Account = "New Account",
                AccountType = "New Type",
                BuyQuantity = 300.0,
                SellQuantity = 150.0,
                BuyPrice = 3000.0,
                SellPrice = 2900.0,
                TradeDate = DateTime.Now,
                TradeSecurity = "New Security",
                TradeStatus = "Active",
                Trader = "New Trader",
                Benchmark = "New Benchmark",
                Book = "New Book",
                CreationName = "System",
                DealName = "New Deal",
                DealType = "Type C",
                Side = "Buy"
            };

            // Act
            await _tradeService.CreateTradeAsAdminAsync(createTradeAdminDTO);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Trade>(trade =>
                trade.Account == createTradeAdminDTO.Account &&
                trade.AccountType == createTradeAdminDTO.AccountType &&
                trade.BuyQuantity == createTradeAdminDTO.BuyQuantity &&
                trade.SellQuantity == createTradeAdminDTO.SellQuantity &&
                trade.BuyPrice == createTradeAdminDTO.BuyPrice &&
                trade.SellPrice == createTradeAdminDTO.SellPrice &&
                trade.TradeSecurity == createTradeAdminDTO.TradeSecurity &&
                trade.Trader == createTradeAdminDTO.Trader &&
                trade.Benchmark == createTradeAdminDTO.Benchmark &&
                trade.Book == createTradeAdminDTO.Book &&
                trade.DealName == createTradeAdminDTO.DealName &&
                trade.DealType == createTradeAdminDTO.DealType &&
                trade.Side == createTradeAdminDTO.Side
            )), Times.Once);

            _tradeRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateTradeAsAdminAsync_ShouldUpdateAndSaveTrade()
        {
            // Arrange
            var updateTradeAdminDTO = new UpdateTradeAdminDTO
            {
                Account = "Updated Account",
                AccountType = "Updated Type",
                BuyQuantity = 300.0,
                SellQuantity = 150.0,
                BuyPrice = 3000.0,
                SellPrice = 2900.0,
                TradeDate = DateTime.Now,
                TradeSecurity = "Updated Security",
                TradeStatus = "Active",
                Trader = "Updated Trader",
                Benchmark = "Updated Benchmark",
                Book = "Updated Book",
                RevisionName = "Updated Revision",
                DealName = "Updated Deal",
                DealType = "Type C",
                Side = "Buy"
            };

            var existingTrade = GetListTradeEntity()[0];

            // Act
            await _tradeService.UpdateTradeAsAdminAsync(updateTradeAdminDTO, existingTrade);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Trade>(trade =>
                trade.Account == updateTradeAdminDTO.Account &&
                trade.AccountType == updateTradeAdminDTO.AccountType &&
                trade.BuyQuantity == updateTradeAdminDTO.BuyQuantity &&
                trade.SellQuantity == updateTradeAdminDTO.SellQuantity &&
                trade.BuyPrice == updateTradeAdminDTO.BuyPrice &&
                trade.SellPrice == updateTradeAdminDTO.SellPrice &&
                trade.TradeSecurity == updateTradeAdminDTO.TradeSecurity &&
                trade.Trader == updateTradeAdminDTO.Trader &&
                trade.Benchmark == updateTradeAdminDTO.Benchmark &&
                trade.Book == updateTradeAdminDTO.Book &&
                trade.DealName == updateTradeAdminDTO.DealName &&
                trade.DealType == updateTradeAdminDTO.DealType &&
                trade.Side == updateTradeAdminDTO.Side
            )), Times.Once);

            _tradeRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTradeByIdAsync_ShouldReturnTrade_WhenIdIsValid()
        {
            var id = 1;

            _tradeRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListTradeEntity()[0]);

            var result = await _tradeService.GetTradeByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Account 1", result.Account);
            Assert.Equal(100.0, result.BuyQuantity);
        }

        [Fact]
        public async Task GetTradeDTOAsAdminByIdAsync_ShouldReturnTradeDTO_WhenIdIsValid()
        {
            var id = 1;

            _tradeRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListTradeEntity()[0]);

            var result = await _tradeService.GetTradeDTOAsAdminByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Account 1", result.Account);
            Assert.Equal(100.0, result.BuyQuantity);
        }

        [Fact]
        public async Task GetTradeDTOAsUserByIdAsync_ShouldReturnTradeDTO_WhenIdIsValid()
        {
            var id = 1;

            _tradeRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListTradeEntity()[0]);

            var result = await _tradeService.GetTradeDTOAsUserByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Security 1", result.TradeSecurity);
            Assert.Equal(100.0, result.BuyQuantity);
        }

        [Fact]
        public async Task DeleteTradeAsync_ShouldCallDeleteAndSave_WhenIdIsValid()
        {
            var tradeId = 1;

            // Act
            await _tradeService.DeleteTradeAsync(tradeId);

            // Assert
            _tradeRepositoryMock.Verify(repo => repo.DeleteAsync(tradeId), Times.Once);
            _tradeRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
