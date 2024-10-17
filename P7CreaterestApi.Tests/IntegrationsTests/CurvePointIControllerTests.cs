using System.Net;
using System.Net.Http.Json;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Domain.DTO.BidDtos;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;
using Xunit;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class CurvePointControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public CurvePointControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<LocalDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"InMemoryCurvePointTestDatabase");
                    });
                   
                });
            });

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
                dbContext.CurvePoints.RemoveRange(dbContext.CurvePoints);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedSampleCurvePointAsync()
        {
            var sampleCurvesPoints = GetSampleCurvePoint();

            foreach (var curvePoint in sampleCurvesPoints)
            {
                await _client.PostAsJsonAsync("/curves/admin", new CreateCurvePointAdminDTO
                {
                     AsOfDate = curvePoint.AsOfDate,
                     CurveDTOId = curvePoint.CurveId,
                     CurvePointDTOValue = curvePoint.CurvePointValue,
                     Term = curvePoint.Term
                });
            }
        }
        

        private List<CurvePoint> GetSampleCurvePoint()
        {
            return new List<CurvePoint>
            {
               new CurvePoint
            {
                CurveId = 1,
                AsOfDate = DateTime.UtcNow.AddDays(2),
                Term = 1.5,
                CurvePointValue = 100
            },
             new CurvePoint
            {
                CurveId = 2,
                AsOfDate = DateTime.UtcNow.AddDays(3),
                Term = 3,
                CurvePointValue = 150
            },
            new CurvePoint
            {
                CurveId = 3,
                AsOfDate = DateTime.UtcNow.AddDays(4),
                Term = 2,
                CurvePointValue = 50
            }
        };
        }


        [Fact]
        public async Task GetCurvePointById_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleCurvePointAsync();

            var allCurveResponse = await _client.GetAsync($"/curves/admin");
            var curves = await allCurveResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            var curveToGet = curves[0];
            // Act
            var response = await _client.GetAsync($"/curves/admin/{curveToGet.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoint = await response.Content.ReadFromJsonAsync<ReadCurvePointAdminDTO>();
            Assert.NotNull(curvePoint);
            Assert.Equal(curveToGet.Id, curvePoint.Id);

            //Dispose
             await ClearDatabase();
        }


        [Fact]
        public async Task GetCurvePointById_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);
            var curvePointId = 1;

            // Act
            var response = await _client.GetAsync($"/curves/admin/{curvePointId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllCurvePoints_AsAdmin_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleCurvePointAsync();
            // Act
            var response = await _client.GetAsync("/curves/admin");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoints = await response.Content.ReadFromJsonAsync<List<ReadCurvePointDTO>>();
            Assert.NotNull(curvePoints);
            Assert.NotEmpty(curvePoints);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateCurvePoint_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var newCurvePoint = new CreateCurvePointAdminDTO
            {
                Term = 1.0,
                CurvePointDTOValue = 100.0,
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow,
            };

            // Act
            var response = await _client.PostAsJsonAsync("/curves/admin", newCurvePoint);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateCurvePoint_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);

            var newCurvePoint = new CreateCurvePointAdminDTO
            {
                Term = 1.0,
                CurvePointDTOValue = 100.0,
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow,
            };

            // Act
            var response = await _client.PostAsJsonAsync("/curves/admin", newCurvePoint);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateCurvePoint_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleCurvePointAsync();
            var allCurveResponse = await _client.GetAsync($"/curves/admin");
            var curves = await allCurveResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            var curveToUpdate = curves[0];
            
            var updatedCurvePoint = new UpdateCurvePointAdminDTO
            {
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow.AddDays(2),
                Term = 3, // Modified
                CurvePointDTOValue = 200 //Modified
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/curves/admin/{curveToUpdate.Id}", updatedCurvePoint);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var allCurveDataResponse = await _client.GetAsync($"/curves/admin");
            var curvesData = await allCurveDataResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            Assert.Equal(updatedCurvePoint.Term, curvesData[0].Term);
            Assert.Equal(updatedCurvePoint.CurvePointDTOValue, curvesData[0].CurvePointDTOValue);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateCurvePoint_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var updatedCurvePoint = new UpdateCurvePointAdminDTO
            {
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow.AddDays(2),
                Term = 3, 
                CurvePointDTOValue = 200 
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/curves/admin/999999", updatedCurvePoint); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteCurvePoint_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleCurvePointAsync();

            var allCurveResponse = await _client.GetAsync("/curves/admin");
            var curves = await allCurveResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            var curveToDelete = curves[0];
            // Act
            var deleteResponse = await _client.DeleteAsync($"/curves/admin/{curveToDelete.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deletedBidResponse = await _client.GetAsync($"/bids/{curveToDelete.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deletedBidResponse.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteCurvePoint_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            // Act
            var response = await _client.DeleteAsync($"/curves/admin/999999"); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllCurvePoints_AsUser_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleCurvePointAsync();

            await _factory.AuthenticateUserAsync(_client);
            // Act
            var response = await _client.GetAsync("/curves");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoints = await response.Content.ReadFromJsonAsync<List<ReadCurvePointDTO>>();
            Assert.NotNull(curvePoints);
            Assert.NotEmpty(curvePoints);

            //Dispose
            await ClearDatabase();
        }

      
    }
}
