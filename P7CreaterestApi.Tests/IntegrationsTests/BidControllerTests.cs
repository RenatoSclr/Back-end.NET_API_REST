using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.BidDtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class BidControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public BidControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task SeedSampleBidsAsync()
        {
            var sampleBids = GetSampleBids();

            foreach (var bid in sampleBids)
            {
                await _factory._client.PostAsJsonAsync("/bids", new CreateBidDTO
                {
                    BidQuantity = bid.BidQuantity,
                    AskQuantity = bid.AskQuantity,
                    BidValue = bid.BidValue,
                    Ask = bid.Ask,
                    Commentary = bid.Commentary,
                    BidSecurity = bid.BidSecurity,
                    Side = bid.Side
                });
            }
        }

        private List<Bid> GetSampleBids()
        {
            return new List<Bid>
            {
                new Bid
                {
                    Account = "Account1",
                    BidType = "Limit",
                    BidQuantity = 100.0,
                    AskQuantity = 50.0,
                    BidValue = 1500.0,
                    Ask = 1600.0,
                    Benchmark = "Benchmark1",
                    BidListDate = DateTime.UtcNow,
                    Commentary = "Test bid 1",
                    BidSecurity = "Security1",
                    BidStatus = "Open",
                    Trader = "Trader1",
                    Book = "Book1",
                    CreationName = "Creator1",
                    CreationDate = DateTime.UtcNow,
                    RevisionName = "Reviser1",
                    RevisionDate = DateTime.UtcNow,
                    DealName = "Deal1",
                    DealType = "Type1",
                    Side = "Buy"
                },
                new Bid
                {
                    Account = "Account2",
                    BidType = "Market",
                    BidQuantity = 200.0,
                    AskQuantity = 100.0,
                    BidValue = 3000.0,
                    Ask = 3200.0,
                    Benchmark = "Benchmark2",
                    BidListDate = DateTime.UtcNow.AddDays(-1),
                    Commentary = "Test bid 2",
                    BidSecurity = "Security2",
                    BidStatus = "Closed",
                    Trader = "Trader2",
                    Book = "Book2",
                    CreationName = "Creator2",
                    CreationDate = DateTime.UtcNow.AddDays(-1),
                    RevisionName = "Reviser2",
                    RevisionDate = DateTime.UtcNow.AddDays(-1),
                    DealName = "Deal2",
                    DealType = "Type2",
                    Side = "Sell"
                },
                new Bid
                {
                    Account = "Account3",
                    BidType = "Stop",
                    BidQuantity = 150.0,
                    AskQuantity = 75.0,
                    BidValue = 2250.0,
                    Ask = 2400.0,
                    Benchmark = "Benchmark3",
                    BidListDate = DateTime.UtcNow.AddDays(-2),
                    Commentary = "Test bid 3",
                    BidSecurity = "Security3",
                    BidStatus = "Pending",
                    Trader = "Trader3",
                    Book = "Book3",
                    CreationName = "Creator3",
                    CreationDate = DateTime.UtcNow.AddDays(-2),
                    RevisionName = "Reviser3",
                    RevisionDate = DateTime.UtcNow.AddDays(-2),
                    DealName = "Deal3",
                    DealType = "Type3",
                    Side = "Buy"
                }
            };
        }

        [Fact]
        public async Task CreateBid_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateUserAsync();

            var newBid = new CreateBidDTO
            {
                BidQuantity = 100.0,
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 1600.0,
                Commentary = "Test bid",
                BidSecurity = "Security1",
                Side = "buy"
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/bids", newBid);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBids_HasData_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.ClearDatabase();

            await _factory.AuthenticateAdminAsync(); 
            await SeedSampleBidsAsync(); 

            // Act
            var response = await _factory._client.GetAsync("/bids");

            // Assert
            response.EnsureSuccessStatusCode();
            var bids = await response.Content.ReadFromJsonAsync<List<ReadBidDTO>>();
            Assert.NotEmpty(bids); 

            var sampleBids = GetSampleBids();
            Assert.Equal(sampleBids.Count, bids.Count);
        }

        [Fact]
        public async Task UpdateBid_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 
            await SeedSampleBidsAsync(); 

            var allBidsResponse = await _factory._client.GetAsync("/bids");
            var bids = await allBidsResponse.Content.ReadFromJsonAsync<List<ReadBidDTO>>();
            var bidToUpdate = bids[0]; 

            var updatedBid = new UpdateBidDTO
            {
                BidQuantity = 100.0,
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 2000, //modified
                Commentary = "Update Bid Commentary", // modified
                BidSecurity = "Security1",
                RevisionName = "Reviser1",
                Side = "Buy"
            };

            // Act
            var response = await _factory._client.PutAsJsonAsync($"/bids/{bidToUpdate.BidId}", updatedBid);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Vérification de la mise à jour
            var updatedBidResponse = await _factory._client.GetAsync($"/bids/{bidToUpdate.BidId}");
            var updatedBidData = await updatedBidResponse.Content.ReadFromJsonAsync<ReadBidDTO>();
            Assert.Equal(updatedBid.Ask, updatedBidData.Ask);
            Assert.Equal(updatedBid.Commentary, updatedBidData.Commentary);
        }

        [Fact]
        public async Task DeleteBid_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(); 
            await SeedSampleBidsAsync(); 

            var allBidsResponse = await _factory._client.GetAsync("/bids");
            var bids = await allBidsResponse.Content.ReadFromJsonAsync<List<ReadBidDTO>>();
            var bidToDelete = bids[0]; 

            // Act
            var response = await _factory._client.DeleteAsync($"/bids/admin/{bidToDelete.BidId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deletedBidResponse = await _factory._client.GetAsync($"/bids/{bidToDelete.BidId}");
            Assert.Equal(HttpStatusCode.NotFound, deletedBidResponse.StatusCode);
        }

        [Fact]
        public async Task GetBidById_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 
            await SeedSampleBidsAsync(); 

            var allBidsResponse = await _factory._client.GetAsync("/bids");
            var bids = await allBidsResponse.Content.ReadFromJsonAsync<List<ReadBidDTO>>();
            var bidToGet = bids[0]; 

            // Act
            var response = await _factory._client.GetAsync($"/bids/{bidToGet.BidId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var bid = await response.Content.ReadFromJsonAsync<ReadBidDTO>();
            Assert.Equal(bidToGet.BidId, bid.BidId); 

            // Vérification des données
            Assert.Equal(bidToGet.BidQuantity, bid.BidQuantity);
            Assert.Equal(bidToGet.AskQuantity, bid.AskQuantity);
            Assert.Equal(bidToGet.BidValue, bid.BidValue);
            Assert.Equal(bidToGet.BidSecurity, bid.BidSecurity);
        }

        [Fact]
        public async Task CreateBid_WithoutRequiredField_ShouldReturnBadRequest()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 

            var invalidBid = new CreateBidDTO
            {
                BidQuantity = -100.0, 
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 1600.0,
                Commentary = "Test bid",
                BidSecurity = "Security1"
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/bids", invalidBid);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBid_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 
            var invalidUpdateBid = new UpdateBidDTO
            {
                BidQuantity = 100.0,
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 2000,
                Commentary = "Update Bid Commentary",
                BidSecurity = "Security1",
                RevisionName = "Reviser1",
                Side = "Buy"
            };

            // Act
            var response = await _factory._client.PutAsJsonAsync("/bids/999999", invalidUpdateBid); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBid_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(); 

            // Act
            var response = await _factory._client.DeleteAsync("/bids/admin/999999"); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetBidById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 

            // Act
            var response = await _factory._client.GetAsync("/bids/999999"); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBidAsAdmin_UserWithoutAdminRole_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(); 

            var newBid = new CreateBidAdminDTO
            {
                BidQuantity = 100.0,
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 1600.0,
                Commentary = "Test admin bid",
                BidSecurity = "Security1",
                Side = "Buy"
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/bids/admin", newBid);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateBidAsAdmin_WithAdminRole_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(); 

            var newBid = new CreateBidAdminDTO
            {
                Account = "Account1",
                BidType = "Limit",
                BidQuantity = 100.0,
                AskQuantity = 50.0,
                BidValue = 1500.0,
                Ask = 1600.0,
                Benchmark = "Benchmark1",
                Commentary = "Test bid 1",
                BidSecurity = "Security1",
                BidStatus = "Open",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = "Creator1",
                DealName = "Deal1",
                DealType = "Type1",
                Side = "Buy"
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/bids/admin", newBid);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBid_NonExistingBid_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();

            // Act
            var response = await _factory._client.DeleteAsync("/bids/admin/999999"); // Id non existant

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
