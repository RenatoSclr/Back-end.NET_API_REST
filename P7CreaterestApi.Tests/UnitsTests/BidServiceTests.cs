using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Moq;
using P7CreateRestApi.Domain.DTO.BidDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class BidServiceTests
    {
        private readonly Mock<IBidRepository> _bidRepositoryMock;
        private readonly BidService _bidService;

        public BidServiceTests()
        {
            _bidRepositoryMock = new Mock<IBidRepository>();
            _bidService = new BidService(_bidRepositoryMock.Object);
        }
        private List<Bid> GetListBidEntity()
        {
            var bidList = new List<Bid>
            {
                new Bid
                {
                    BidId = 1,
                    Account = "GlobalFinance",
                    BidType = "Auction",
                    BidQuantity = 1000000.0,  // 1 million units
                    AskQuantity = 900000.0,   // 900,000 units
                    BidValue = 50000.0,       // $50,000
                    Ask = 49000.0,            // $49,000
                    Benchmark = "LIBOR",
                    BidListDate = DateTime.Now.AddDays(-1),  // Yesterday
                    Commentary = "Strong bid, competitive pricing",
                    BidSecurity = "US Treasury Bonds",
                    BidStatus = "Open",
                    Trader = "John Doe",
                    Book = "Fixed Income",
                    CreationName = "System",
                    CreationDate = DateTime.Now.AddDays(-7), // Created 7 days ago
                    RevisionName = "Jane Smith",
                    RevisionDate = DateTime.Now,             // Revised today
                    DealName = "Bond Auction 2024",
                    DealType = "Sovereign Bonds",
                    Side = "Buy"
                },
                new Bid
                {
                    BidId = 2,
                    Account = "CapitalInvest",
                    BidType = "Private Sale",
                    BidQuantity = 2000000.0,  // 2 million units
                    AskQuantity = 1800000.0,  // 1.8 million units
                    BidValue = 75000.0,       // $75,000
                    Ask = 74000.0,            // $74,000
                    Benchmark = "Euribor",
                    BidListDate = DateTime.Now.AddDays(-5),  // 5 days ago
                    Commentary = "Steady demand, awaiting market shift",
                    BidSecurity = "Corporate Bonds",
                    BidStatus = "Pending",
                    Trader = "Alice Johnson",
                    Book = "Equities",
                    CreationName = "System",
                    CreationDate = DateTime.Now.AddDays(-10), // Created 10 days ago
                    RevisionName = "David Lee",
                    RevisionDate = DateTime.Now.AddDays(-2),  // Revised 2 days ago
                    DealName = "Corporate Bond Sale",
                    DealType = "Corporate Bonds",
                    Side = "Sell"
                },
                new Bid
                {
                    BidId = 3,
                    Account = "WealthPartners",
                    BidType = "Direct Purchase",
                    BidQuantity = 1500000.0,  // 1.5 million units
                    AskQuantity = 1400000.0,  // 1.4 million units
                    BidValue = 60000.0,       // $60,000
                    Ask = 59000.0,            // $59,000
                    Benchmark = "SOFR",
                    BidListDate = DateTime.Now.AddDays(-3),  // 3 days ago
                    Commentary = "High interest in safe assets",
                    BidSecurity = "Municipal Bonds",
                    BidStatus = "Closed",
                    Trader = "Michael Brown",
                    Book = "Municipal Debt",
                    CreationName = "System",
                    CreationDate = DateTime.Now.AddDays(-15), // Created 15 days ago
                    RevisionName = "Laura Green",
                    RevisionDate = DateTime.Now.AddDays(-1),  // Revised 1 day ago
                    DealName = "City Bonds Purchase",
                    DealType = "Municipal Bonds",
                    Side = "Buy"
                }
            };

            return bidList;
        }




        [Fact]
        public async Task GetAllBidDTOsAsAdminAsync_ShouldReturnBidDTOList_WhenBidsExist()
        {
            // Arrange
            _bidRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListBidEntity());

            //Act
            var result = await _bidService.GetAllBidDTOsAsAdminAsync();

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(3, result.Count); 
            Assert.Equal("GlobalFinance", result[0].Account); 
            Assert.Equal("CapitalInvest", result[1].Account);
        }

        [Fact]
        public async Task GetAllBid_ReturnCorrectDTOListAsUser()
        {
            // Arrange
            _bidRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListBidEntity());

            //Act
            var result = await _bidService.GetAllBidDTOsAsUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(50000, result[0].BidValue);
            Assert.Equal(75000, result[1].BidValue);
            Assert.Equal(60000, result[2].BidValue);
        }

        [Fact]
        public async Task CreateBid_ShouldAddAndSaveBidAsAdmin()
        {
            // Arrange
            var createBidAdminDTO = new CreateBidAdminDTO
            {
                Account = "GlobalFinance",
                BidType = "Auction",
                Trader = "John Doe",
                BidStatus = "Open",
                Book = "Fixed Income",
                BidQuantity = 1000000.0,  // 1 million units
                AskQuantity = 900000.0,   // 900,000 units
                BidValue = 50000.0,       // $50,000
                Ask = 49000.0,            // $49,000
                Benchmark = "LIBOR",
                CreationName = "System",
                BidSecurity = "US Treasury Bonds",
                Commentary = "Strong bid, competitive pricing",
                DealName = "Bond Auction 2024",
                DealType = "Sovereign Bonds",
                Side = "Buy"
            };

            //Act
            await _bidService.CreateBidAsAdminAsync(createBidAdminDTO);

            // Assert
            _bidRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Bid>(b =>
               b.Account == createBidAdminDTO.Account &&
               b.BidType == createBidAdminDTO.BidType &&
               b.Trader == createBidAdminDTO.Trader &&
               b.BidStatus == createBidAdminDTO.BidStatus &&
               b.Book == createBidAdminDTO.Book &&
               b.BidQuantity == createBidAdminDTO.BidQuantity &&
               b.AskQuantity == createBidAdminDTO.AskQuantity &&
               b.BidValue == createBidAdminDTO.BidValue &&
               b.Ask == createBidAdminDTO.Ask &&
               b.Benchmark == createBidAdminDTO.Benchmark &&
               b.CreationName == createBidAdminDTO.CreationName &&
               b.BidSecurity == createBidAdminDTO.BidSecurity &&
               b.Commentary == createBidAdminDTO.Commentary &&
               b.DealName == createBidAdminDTO.DealName &&
               b.DealType == createBidAdminDTO.DealType &&
               b.Side == createBidAdminDTO.Side

           )), Times.Once);

            _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateBid_ShouldAddAndSaveBidAsUser()
        {
            // Arrange
            var createBidUserDTO = new CreateBidDTO
            {
                BidQuantity = 1000000.0,  // 1 million units
                AskQuantity = 900000.0,   // 900,000 units
                BidValue = 50000.0,       // $50,000
                Ask = 49000.0,            // $49,000
                CreationName = "System",
                BidSecurity = "US Treasury Bonds",
                Commentary = "Strong bid, competitive pricing",
                Side = "Buy"
            };

            //Act
            await _bidService.CreateBidAsUserAsync(createBidUserDTO);

            // Assert
            _bidRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Bid>(b =>
               b.BidQuantity == createBidUserDTO.BidQuantity &&
               b.AskQuantity == createBidUserDTO.AskQuantity &&
               b.BidValue == createBidUserDTO.BidValue &&
               b.Ask == createBidUserDTO.Ask &&
               b.CreationName == createBidUserDTO.CreationName &&
               b.BidSecurity == createBidUserDTO.BidSecurity &&
               b.Commentary == createBidUserDTO.Commentary &&
               b.Side == createBidUserDTO.Side

           )), Times.Once);

            _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBid_ShouldUpdateAndSaveBidAsAdmin()
        {
            // Arrange
            var updateBidAdminDTO = new UpdateBidAdminDTO
            {
                Account = "GlobalFinance",
                BidType = "Auction",
                BidQuantity = 1000000.0,  // 1 million units
                AskQuantity = 900000.0,   // 900,000 units
                BidValue = 50000.0,       // $50,000
                Ask = 49000.0,            // $49,000
                Benchmark = "LIBOR",
                Commentary = "Strong bid, competitive pricing",
                BidSecurity = "US Treasury Bonds",
                BidStatus = "Open",
                Trader = "John Doe",
                Book = "Fixed Income",
                RevisionName = "Lara",        
                DealName = "Bond Auction 2024",
                DealType = "Sovereign Bonds",
                Side = "Buy"
            };

            //Act
            await _bidService.UpdateBidAsAdminAsync(updateBidAdminDTO, GetListBidEntity()[0]);

            // Assert
            _bidRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Bid>(b =>
               b.Account == updateBidAdminDTO.Account &&
               b.BidType == updateBidAdminDTO.BidType &&
               b.Trader == updateBidAdminDTO.Trader &&
               b.BidStatus == updateBidAdminDTO.BidStatus &&
               b.Book == updateBidAdminDTO.Book &&
               b.BidQuantity == updateBidAdminDTO.BidQuantity &&
               b.AskQuantity == updateBidAdminDTO.AskQuantity &&
               b.BidValue == updateBidAdminDTO.BidValue &&
               b.Ask == updateBidAdminDTO.Ask &&
               b.Benchmark == updateBidAdminDTO.Benchmark &&
               b.RevisionName == updateBidAdminDTO.RevisionName &&
               b.BidSecurity == updateBidAdminDTO.BidSecurity &&
               b.Commentary == updateBidAdminDTO.Commentary &&
               b.DealName == updateBidAdminDTO.DealName &&
               b.DealType == updateBidAdminDTO.DealType &&
               b.Side == updateBidAdminDTO.Side

           )), Times.Once);

            _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBid_ShouldUpdateAndSaveBidAsUser()
        {
            // Arrange
            var updateBidUserDTO = new UpdateBidDTO
            {
                BidQuantity = 1000000.0,  // 1 million units
                AskQuantity = 900000.0,   // 900,000 units
                BidValue = 50000.0,       // $50,000
                Ask = 49000.0,            // $49,000
                Commentary = "Strong bid, competitive pricing",
                BidSecurity = "US Treasury Bonds",
                RevisionName = "Lara",
                Side = "Buy"
            };

            //Act
            await _bidService.UpdateBidAsUserAsync(updateBidUserDTO, GetListBidEntity()[0]);

            // Assert
            _bidRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Bid>(b =>
               b.BidQuantity == updateBidUserDTO.BidQuantity &&
               b.AskQuantity == updateBidUserDTO.AskQuantity &&
               b.BidValue == updateBidUserDTO.BidValue &&
               b.Ask == updateBidUserDTO.Ask &&
               b.RevisionName == updateBidUserDTO.RevisionName &&
               b.BidSecurity == updateBidUserDTO.BidSecurity &&
               b.Commentary == updateBidUserDTO.Commentary &&
               b.Side == updateBidUserDTO.Side

           )), Times.Once);

            _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBid_ShouldGetBidAsAdmin()
        {
            var id = 1;

            _bidRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).
                ReturnsAsync(GetListBidEntity()[0]);

            var result = await _bidService.GetBidAdminDTOByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(50000, result.BidValue);
            Assert.Equal("GlobalFinance", result.Account);
            Assert.Equal("LIBOR", result.Benchmark);

        }

        [Fact]
        public async Task GetBid_ShouldGetBidAsUser()
        {
            var id = 1;

            _bidRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).
                ReturnsAsync(GetListBidEntity()[0]);

            var result = await _bidService.GetBidDTOByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(50000, result.BidValue);
            Assert.Equal(1000000.0, result.BidQuantity);
            Assert.Equal(900000.0, result.AskQuantity);
            Assert.Equal(50000, result.BidValue);
            Assert.Equal("Jane Smith", result.RevisionName);
        }

        [Fact]
        public async Task DeleteBid_ShouldCallDeleteAndSave_WhenIdIsValid()
        {
            int bidId = 1;
            await _bidService.DeleteBidAsync(bidId);

            _bidRepositoryMock.Verify(repo => repo.DeleteAsync(bidId), Times.Once);

            _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
