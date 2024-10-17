using System.Net;
using System.Net.Http.Json;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Domain.DTO.RatingDtos;


namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class RatingControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public RatingControllerTests(CustomWebApplicationFactory factory)
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
                dbContext.Ratings.RemoveRange(dbContext.Ratings);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedSampleRatingsAsync()
        {
            var sampleRatings = GetSampleRatings();

            foreach (var rating in sampleRatings)
            {
                await _client.PostAsJsonAsync("/ratings/admin", new EditRatingAdminDTO
                {
                    MoodysRating = rating.MoodysRating,
                    SandPRating = rating.SandPRating,
                    FitchRating = rating.FitchRating,
                    OrderNumber = rating.OrderNumber
                });
            }
        }

        private List<EditRatingAdminDTO> GetSampleRatings()
        {
            return new List<EditRatingAdminDTO>
            {
                new EditRatingAdminDTO
                {
                    MoodysRating = "A1",
                    SandPRating = "AA",
                    FitchRating = "F1",
                    OrderNumber = 1
                },
                new EditRatingAdminDTO
                {
                    MoodysRating = "Baa2",
                    SandPRating = "BBB",
                    FitchRating = "F3",
                    OrderNumber = 2
                },
                new EditRatingAdminDTO
                {
                    MoodysRating = "Caa1",
                    SandPRating = "CCC",
                    FitchRating = "F4",
                    OrderNumber = 3
                }
            };
        }

        [Fact]
        public async Task GetRatingById_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRatingsAsync();

            var allRatingsResponse = await _client.GetAsync("/ratings");
            var ratings = await allRatingsResponse.Content.ReadFromJsonAsync<List<ReadRatingDTO>>();
            var ratingToGet = ratings[0];

            // Act
            var response = await _client.GetAsync($"/ratings/{ratingToGet.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var rating = await response.Content.ReadFromJsonAsync<ReadRatingDTO>();
            Assert.NotNull(rating);
            Assert.Equal(ratingToGet.Id, rating.Id);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task GetRatingById_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);
            var ratingId = 1;

            // Act
            var response = await _client.GetAsync($"/ratings/{ratingId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllRatings_AsAdmin_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRatingsAsync();

            // Act
            var response = await _client.GetAsync("/ratings");

            // Assert
            response.EnsureSuccessStatusCode();
            var ratings = await response.Content.ReadFromJsonAsync<List<ReadRatingDTO>>();
            Assert.NotNull(ratings);
            Assert.NotEmpty(ratings);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateRating_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var newRating = new EditRatingAdminDTO
            {
                MoodysRating = "A1",
                SandPRating = "AA",
                FitchRating = "F1",
                OrderNumber = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/ratings/admin", newRating);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateRating_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);

            var newRating = new EditRatingAdminDTO
            {
                MoodysRating = "A1",
                SandPRating = "AA",
                FitchRating = "F1",
                OrderNumber = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/ratings/admin", newRating);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateRating_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRatingsAsync();

            var allRatingsResponse = await _client.GetAsync("/ratings");
            var ratings = await allRatingsResponse.Content.ReadFromJsonAsync<List<ReadRatingDTO>>();
            var ratingToUpdate = ratings[0];

            var updatedRating = new EditRatingAdminDTO
            {
                MoodysRating = "A2", // Modified
                SandPRating = "A",
                FitchRating = "F2",
                OrderNumber = 2 // Modified
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/ratings/admin/{ratingToUpdate.Id}", updatedRating);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedRatingResponse = await _client.GetAsync($"/ratings/{ratingToUpdate.Id}");
            var updatedRatingData = await updatedRatingResponse.Content.ReadFromJsonAsync<ReadRatingDTO>();
            Assert.Equal(updatedRating.MoodysRating, updatedRatingData.MoodysRating);
            Assert.Equal(updatedRating.OrderNumber, updatedRatingData.OrderNumber);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateRating_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var updatedRating = new EditRatingAdminDTO
            {
                MoodysRating = "A1",
                SandPRating = "AA",
                FitchRating = "F1",
                OrderNumber = 1
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/ratings/admin/999999", updatedRating); // Invalid ID

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteRating_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRatingsAsync();

            var allRatingsResponse = await _client.GetAsync("/ratings");
            var ratings = await allRatingsResponse.Content.ReadFromJsonAsync<List<ReadRatingDTO>>();
            var ratingToDelete = ratings[0];

            // Act
            var deleteResponse = await _client.DeleteAsync($"/ratings/admin/{ratingToDelete.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deletedRatingResponse = await _client.GetAsync($"/ratings/{ratingToDelete.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deletedRatingResponse.StatusCode);

            //Dispose 
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteRating_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            // Act
            var response = await _client.DeleteAsync($"/ratings/admin/999999"); // Invalid ID

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose 
            await ClearDatabase();
        }
    }
}
