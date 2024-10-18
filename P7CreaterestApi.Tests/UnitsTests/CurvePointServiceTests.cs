using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services;
using Moq;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class CurvePointServiceTests
    {
        private readonly Mock<ICurvePointRepository> _curvePointRepositoryMock;
        private readonly CurvePointService _curvePointService;

        public CurvePointServiceTests()
        {
            _curvePointRepositoryMock = new Mock<ICurvePointRepository>();
            _curvePointService = new CurvePointService(_curvePointRepositoryMock.Object);
        }

        private List<CurvePoint> GetListCurvePointEntity()
        {
            return new List<CurvePoint>
            {
                new CurvePoint
                {
                    Id = 1,
                    CurveId = 101,
                    AsOfDate = DateTime.Now.AddDays(-2),  
                    Term = 1.5,  
                    CurvePointValue = 2.5,  
                    CreationDate = DateTime.Now.AddDays(-7)  
                },
                new CurvePoint
                {
                    Id = 2,
                    CurveId = 102,
                    AsOfDate = DateTime.Now.AddDays(-1), 
                    Term = 5.0,  
                    CurvePointValue = 3.2,  
                    CreationDate = DateTime.Now.AddDays(-10)  
                },
                new CurvePoint
                {
                    Id = 3,
                    CurveId = 103,
                    AsOfDate = DateTime.Now,  
                    Term = 10.0,  
                    CurvePointValue = 4.0,  
                    CreationDate = DateTime.Now.AddDays(-30)  
                }
            };
        }

        [Fact]
        public async Task GetAllCurvePointDTOsAsAdminAsync_ShouldReturnCurvePointDTOList_WhenCurvePointsExist()
        {
            // Arrange
            _curvePointRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListCurvePointEntity());

            // Act
            var result = await _curvePointService.GetAllCurvePointDTOsAsAdminAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(1.5, result[0].Term);
            Assert.Equal(5, result[1].Term);
            Assert.Equal(10, result[2].Term);
        }

        [Fact]
        public async Task GetAllCurvePointDTOsAsUserAsync_ShouldReturnCurvePointDTOList_WhenCurvePointsExist()
        {
            // Arrange
            _curvePointRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListCurvePointEntity());

            // Act
            var result = await _curvePointService.GetAllCurvePointDTOsAsUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(2.5, result[0].CurvePointDTOValue);
            Assert.Equal(3.2, result[1].CurvePointDTOValue);
            Assert.Equal(4.0, result[2].CurvePointDTOValue);
        }

        [Fact]
        public async Task CreateCurvePointAsAdmin_ShouldAddAndSaveCurvePoint()
        {
            // Arrange
            var createCurvePointAdminDTO = new CreateCurvePointAdminDTO
            {
                CurveDTOId = 101,
                AsOfDate = DateTime.Now.AddDays(-2),
                Term = 1.5,
                CurvePointDTOValue = 2.5
            };

            // Act
            await _curvePointService.CreateCurvePointAsAdminAsync(createCurvePointAdminDTO);

            // Assert
            _curvePointRepositoryMock.Verify(repo => repo.AddAsync(It.Is<CurvePoint>(cp =>
               cp.CurveId == createCurvePointAdminDTO.CurveDTOId &&
               cp.AsOfDate == createCurvePointAdminDTO.AsOfDate &&
               cp.Term == createCurvePointAdminDTO.Term &&
               cp.CurvePointValue == createCurvePointAdminDTO.CurvePointDTOValue
           )), Times.Once);

            _curvePointRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCurvePointAsAdmin_ShouldUpdateAndSaveCurvePoint()
        {
            // Arrange
            var updateCurvePointAdminDTO = new UpdateCurvePointAdminDTO
            {
                CurveDTOId = 101,
                AsOfDate = DateTime.Now.AddDays(-2),
                Term = 2.0,
                CurvePointDTOValue = 2.7
            };

            var existingCurvePoint = GetListCurvePointEntity()[0];

            // Act
            await _curvePointService.UpdateCurvePointAsAdminAsync(updateCurvePointAdminDTO, existingCurvePoint);

            // Assert
            _curvePointRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<CurvePoint>(cp =>
               cp.CurveId == updateCurvePointAdminDTO.CurveDTOId &&
               cp.AsOfDate == updateCurvePointAdminDTO.AsOfDate &&
               cp.Term == updateCurvePointAdminDTO.Term &&
               cp.CurvePointValue == updateCurvePointAdminDTO.CurvePointDTOValue
           )), Times.Once);

            _curvePointRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCurvePointById_ShouldReturnCurvePoint_WhenIdIsValid()
        {
            var id = 1;

            _curvePointRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListCurvePointEntity()[0]);

            var result = await _curvePointService.GetCurvePointByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(2.5, result.CurvePointValue);
            Assert.Equal(101, result.CurveId);
        }

        [Fact]
        public async Task DeleteCurvePoint_ShouldCallDeleteAndSave_WhenIdIsValid()
        {
            int curvePointId = 1;

            await _curvePointService.DeleteCurvePointAsync(curvePointId);

            _curvePointRepositoryMock.Verify(repo => repo.DeleteAsync(curvePointId), Times.Once);

            _curvePointRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
