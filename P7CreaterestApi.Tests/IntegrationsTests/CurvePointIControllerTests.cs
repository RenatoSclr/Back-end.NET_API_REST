using System.Net;
using System.Net.Http.Json;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.BidDtos;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;
using Xunit;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class CurvePointControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CurvePointControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private async Task SeedSampleCurvePointAsync()
        {
            var sampleCurvesPoints = GetSampleCurvePoint();

            foreach (var curvePoint in sampleCurvesPoints)
            {
                await _factory._client.PostAsJsonAsync("/curves/admin", new CreateCurvePointAdminDTO
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
            await _factory.AuthenticateAdminAsync();
            await SeedSampleCurvePointAsync();

            var allCurveResponse = await _factory._client.GetAsync($"/curves/admin");
            var curves = await allCurveResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            var curveToGet = curves[0];
            // Act
            var response = await _factory._client.GetAsync($"/curves/admin/{curveToGet.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoint = await response.Content.ReadFromJsonAsync<ReadCurvePointAdminDTO>();
            Assert.NotNull(curvePoint);
            Assert.Equal(curveToGet.Id, curvePoint.Id);
        }

        [Fact]
        public async Task GetCurvePointById_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync();
            var curvePointId = 1;

            // Act
            var response = await _factory._client.GetAsync($"/curves/admin/{curvePointId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode); 
        }

        [Fact]
        public async Task GetAllCurvePoints_AsAdmin_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();
            await SeedSampleCurvePointAsync();
            // Act
            var response = await _factory._client.GetAsync("/curves/admin");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoints = await response.Content.ReadFromJsonAsync<List<ReadCurvePointDTO>>();
            Assert.NotNull(curvePoints);
            Assert.NotEmpty(curvePoints); 
        }

        [Fact]
        public async Task CreateCurvePoint_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();

            var newCurvePoint = new CreateCurvePointAdminDTO
            {
                Term = 1.0,
                CurvePointDTOValue = 100.0,
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow,
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/curves/admin", newCurvePoint);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateCurvePoint_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync();

            var newCurvePoint = new CreateCurvePointAdminDTO
            {
                Term = 1.0,
                CurvePointDTOValue = 100.0,
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow,
            };

            // Act
            var response = await _factory._client.PostAsJsonAsync("/curves/admin", newCurvePoint);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCurvePoint_AsAdmin_ShouldReturnOk()
        {
            await _factory.ClearDatabase();
            // Arrange
            await _factory.AuthenticateAdminAsync();
            await SeedSampleCurvePointAsync();
            var allCurveResponse = await _factory._client.GetAsync($"/curves/admin");
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
            var response = await _factory._client.PutAsJsonAsync($"/curves/admin/{curveToUpdate.Id}", updatedCurvePoint);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var allCurveDataResponse = await _factory._client.GetAsync($"/curves/admin");
            var curvesData = await allCurveDataResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            Assert.Equal(updatedCurvePoint.Term, curvesData[0].Term);
            Assert.Equal(updatedCurvePoint.CurvePointDTOValue, curvesData[0].CurvePointDTOValue);
        }

        [Fact]
        public async Task UpdateCurvePoint_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();

            var updatedCurvePoint = new UpdateCurvePointAdminDTO
            {
                CurveDTOId = 1,
                AsOfDate = DateTime.UtcNow.AddDays(2),
                Term = 3, 
                CurvePointDTOValue = 200 
            };

            // Act
            var response = await _factory._client.PutAsJsonAsync($"/curves/admin/999999", updatedCurvePoint); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();
            await SeedSampleCurvePointAsync();

            var allCurveResponse = await _factory._client.GetAsync("/curves/admin");
            var curves = await allCurveResponse.Content.ReadFromJsonAsync<List<ReadCurvePointAdminDTO>>();
            var curveToDelete = curves[0];
            // Act
            var deleteResponse = await _factory._client.DeleteAsync($"/curves/admin/{curveToDelete.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deletedBidResponse = await _factory._client.GetAsync($"/bids/{curveToDelete.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deletedBidResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteCurvePoint_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();

            // Act
            var response = await _factory._client.DeleteAsync($"/curves/admin/999999"); 

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllCurvePoints_AsUser_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync();
            await SeedSampleCurvePointAsync();

            await _factory.AuthenticateUserAsync();
            // Act
            var response = await _factory._client.GetAsync("/curves");

            // Assert
            response.EnsureSuccessStatusCode();
            var curvePoints = await response.Content.ReadFromJsonAsync<List<ReadCurvePointDTO>>();
            Assert.NotNull(curvePoints);
            Assert.NotEmpty(curvePoints);
        }
    }
}
