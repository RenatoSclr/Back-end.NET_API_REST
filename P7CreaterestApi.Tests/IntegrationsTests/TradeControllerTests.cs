using System.Net;
using System.Net.Http.Json;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.TradeDtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class TradeControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public TradeControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private async Task ClearDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
                dbContext.Trades.RemoveRange(dbContext.Trades);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedSampleTradesAsync()
        {
            var sampleTrades = GetSampleTrades();

            foreach (var trade in sampleTrades)
            {
                await _client.PostAsJsonAsync("/trade/admin", new CreateTradeAdminDTO
                {
                    Account = trade.Account,
                    AccountType = trade.AccountType,
                    BuyQuantity = trade.BuyQuantity,
                    SellQuantity = trade.SellQuantity,
                    TradeSecurity = trade.TradeSecurity,
                    TradeStatus = trade.TradeStatus,
                    Trader = trade.Trader,
                    Benchmark = trade.Benchmark,
                    Book = trade.Book,
                    Side = trade.Side,
                    TradeDate = trade.TradeDate
                });
            }
        }

        private List<Trade> GetSampleTrades()
        {
            return new List<Trade>
            {
                new Trade
                {
                    Account = "Account1",
                    AccountType = "Type1",
                    BuyQuantity = 100.0,
                    SellQuantity = 50.0,
                    TradeSecurity = "Security1",
                    TradeStatus = "Open",
                    Trader = "Trader1",
                    Benchmark = "Benchmark1",
                    Book = "Book1",
                    Side = "Buy",
                    TradeDate = DateTime.UtcNow
                },
                new Trade
                {
                    Account = "Account2",
                    AccountType = "Type2",
                    BuyQuantity = 200.0,
                    SellQuantity = 100.0,
                    TradeSecurity = "Security2",
                    TradeStatus = "Closed",
                    Trader = "Trader2",
                    Benchmark = "Benchmark2",
                    Book = "Book2",
                    Side = "Sell",
                    TradeDate = DateTime.UtcNow
                }
            };
        }

        [Fact]
        public async Task GetTradeById_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();

            var allTradesResponse = await _client.GetAsync("/trade/admin");
            var trades = await allTradesResponse.Content.ReadFromJsonAsync<List<ReadTradeAdminDTO>>();
            var tradeToGet = trades[0];

            // Act
            var response = await _client.GetAsync($"/trade/admin/{tradeToGet.TradeId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var trade = await response.Content.ReadFromJsonAsync<ReadTradeAdminDTO>();
            Assert.NotNull(trade);
            Assert.Equal(tradeToGet.TradeId, trade.TradeId);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetTradeById_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();
            await _factory.AuthenticateUserAsync(_client);


            var allTradesResponse = await _client.GetAsync("/trade");
            var trades = await allTradesResponse.Content.ReadFromJsonAsync<List<ReadTradeDTO>>();
            var tradeToGet = trades[0];

            // Act
            var response = await _client.GetAsync($"/trade/{tradeToGet.TradeId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var trade = await response.Content.ReadFromJsonAsync<ReadTradeDTO>();
            Assert.NotNull(trade);
            Assert.Equal(tradeToGet.TradeId, trade.TradeId);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetTradeById_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);
            var tradeId = 1;

            // Act
            var response = await _client.GetAsync($"/trade/admin/{tradeId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllTrades_AsAdmin_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();

            // Act
            var response = await _client.GetAsync("/trade/admin");

            // Assert
            response.EnsureSuccessStatusCode();
            var trades = await response.Content.ReadFromJsonAsync<List<ReadTradeAdminDTO>>();
            Assert.NotNull(trades);
            Assert.NotEmpty(trades);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllTrades_AsUser_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();
            await _factory.AuthenticateUserAsync(_client);

            // Act
            var response = await _client.GetAsync("/trade");

            // Assert
            response.EnsureSuccessStatusCode();
            var trades = await response.Content.ReadFromJsonAsync<List<ReadTradeDTO>>();
            Assert.NotNull(trades);
            Assert.NotEmpty(trades);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateTrade_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var newTrade = new CreateTradeAdminDTO
            {
                Account = "Account1",
                AccountType = "Type1",
                BuyQuantity = 100.0,
                SellQuantity = 50.0,
                TradeSecurity = "Security1",
                TradeStatus = "Open",
                Trader = "Trader1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                Side = "Buy",
                TradeDate = DateTime.UtcNow
            };

            // Act
            var response = await _client.PostAsJsonAsync("/trade/admin", newTrade);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateTrade_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);

            var newTrade = new CreateTradeDTO
            {
                BuyQuantity = 100.0,
                SellQuantity = 50.0,
                TradeSecurity = "Security1",
                Side = "Buy"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/trade", newTrade);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateTrade_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);

            var newTrade = new CreateTradeAdminDTO
            {
                Account = "Account1",
                AccountType = "Type1",
                BuyQuantity = 100.0,
                SellQuantity = 50.0,
                TradeSecurity = "Security1",
                TradeStatus = "Open",
                Trader = "Trader1",
                Benchmark = "Benchmark1",
                Book = "Book1",
                Side = "Buy",
                TradeDate = DateTime.UtcNow
            };

            // Act
            var response = await _client.PostAsJsonAsync("/trade/admin", newTrade);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateTrade_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();

            var allTradesResponse = await _client.GetAsync("/trade/admin");
            var trades = await allTradesResponse.Content.ReadFromJsonAsync<List<ReadTradeAdminDTO>>();
            var tradeToUpdate = trades[0];

            var updatedTrade = new UpdateTradeAdminDTO
            {
                Account = "Updated Account",
                AccountType = "Updated Type",
                BuyQuantity = 150.0,
                SellQuantity = 75.0,
                TradeSecurity = "Updated Security",
                TradeStatus = "Closed",
                Trader = "Updated Trader",
                Benchmark = "Updated Benchmark",
                Book = "Updated Book",
                Side = "Sell",
                TradeDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/trade/admin/{tradeToUpdate.TradeId}", updatedTrade);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedTradeResponse = await _client.GetAsync($"/trade/admin/{tradeToUpdate.TradeId}");
            var updatedTradeData = await updatedTradeResponse.Content.ReadFromJsonAsync<ReadTradeAdminDTO>();
            Assert.Equal(updatedTrade.Account, updatedTradeData.Account);
            Assert.Equal(updatedTrade.TradeStatus, updatedTradeData.TradeStatus);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateTrade_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var updatedTrade = new UpdateTradeAdminDTO
            {
                Account = "Updated Account",
                AccountType = "Updated Type",
                BuyQuantity = 150.0,
                SellQuantity = 75.0,
                TradeSecurity = "Updated Security",
                TradeStatus = "Closed",
                Trader = "Updated Trader",
                Benchmark = "Updated Benchmark",
                Book = "Updated Book",
                Side = "Sell",
                TradeDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/trade/admin/999999", updatedTrade);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteTrade_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleTradesAsync();

            var allTradesResponse = await _client.GetAsync("/trade/admin");
            var trades = await allTradesResponse.Content.ReadFromJsonAsync<List<ReadTradeAdminDTO>>();
            var tradeToDelete = trades[0];

            // Act
            var deleteResponse = await _client.DeleteAsync($"/trade/admin/{tradeToDelete.TradeId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deletedTradeResponse = await _client.GetAsync($"/trade/admin/{tradeToDelete.TradeId}");
            Assert.Equal(HttpStatusCode.NotFound, deletedTradeResponse.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteTrade_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            // Act
            var response = await _client.DeleteAsync($"/trade/admin/999999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }
    }
}
