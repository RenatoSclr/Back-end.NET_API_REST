using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Moq;
using P7CreateRestApi.Domain.DTO.RatingDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _ratingRepositoryMock;
        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _ratingRepositoryMock = new Mock<IRatingRepository>();
            _ratingService = new RatingService(_ratingRepositoryMock.Object);
        }

        private List<Rating> GetListRatingEntity()
        {
            return new List<Rating>
            {
                new Rating
                {
                    Id = 1,
                    MoodysRating = "Aaa",
                    SandPRating = "AAA",
                    FitchRating = "AAA",
                    OrderNumber = 1
                },
                new Rating
                {
                    Id = 2,
                    MoodysRating = "Aa1",
                    SandPRating = "AA+",
                    FitchRating = "AA+",
                    OrderNumber = 2
                },
                new Rating
                {
                    Id = 3,
                    MoodysRating = "Aa2",
                    SandPRating = "AA",
                    FitchRating = "AA",
                    OrderNumber = 3
                }
            };
        }

        [Fact]
        public async Task GetAllRatingDTOsAsync_ShouldReturnRatingDTOList_WhenRatingsExist()
        {
            // Arrange
            _ratingRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListRatingEntity());

            // Act
            var result = await _ratingService.GetAllRatingDTOsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Aaa", result[0].MoodysRating);
            Assert.Equal("Aa1", result[1].MoodysRating);
            Assert.Equal("Aa2", result[2].MoodysRating);
        }

        [Fact]
        public async Task CreateRatingAsync_ShouldAddAndSaveRating()
        {
            // Arrange
            var editRatingAdminDTO = new EditRatingAdminDTO
            {
                MoodysRating = "Aaa",
                SandPRating = "AAA",
                FitchRating = "AAA",
                OrderNumber = 1
            };

            // Act
            await _ratingService.CreateRatingAsync(editRatingAdminDTO);

            // Assert
            _ratingRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Rating>(r =>
                r.MoodysRating == editRatingAdminDTO.MoodysRating &&
                r.SandPRating == editRatingAdminDTO.SandPRating &&
                r.FitchRating == editRatingAdminDTO.FitchRating &&
                r.OrderNumber == editRatingAdminDTO.OrderNumber
            )), Times.Once);

            _ratingRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateRatingAsync_ShouldUpdateAndSaveRating()
        {
            // Arrange
            var editRatingAdminDTO = new EditRatingAdminDTO
            {
                MoodysRating = "Aa1",
                SandPRating = "AA+",
                FitchRating = "AA+",
                OrderNumber = 2
            };

            var existingRating = GetListRatingEntity()[0];

            // Act
            await _ratingService.UpdateRatingAsync(editRatingAdminDTO, existingRating);

            // Assert
            _ratingRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Rating>(r =>
                r.MoodysRating == editRatingAdminDTO.MoodysRating &&
                r.SandPRating == editRatingAdminDTO.SandPRating &&
                r.FitchRating == editRatingAdminDTO.FitchRating &&
                r.OrderNumber == editRatingAdminDTO.OrderNumber
            )), Times.Once);

            _ratingRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetRatingById_ShouldReturnRating_WhenIdIsValid()
        {
            var id = 1;

            _ratingRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListRatingEntity()[0]);

            var result = await _ratingService.GetRatingByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Aaa", result.MoodysRating);
            Assert.Equal("AAA", result.SandPRating);
        }

        [Fact]
        public async Task GetRatingDTOById_ShouldReturnRatingDTO_WhenIdIsValid()
        {
            var id = 1;

            _ratingRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListRatingEntity()[0]);

            var result = await _ratingService.GetRatingDTOByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Aaa", result.MoodysRating);
            Assert.Equal("AAA", result.SandPRating);
        }

        [Fact]
        public async Task DeleteRatingAsync_ShouldCallDeleteAndSave_WhenIdIsValid()
        {
            int ratingId = 1;

            await _ratingService.DeleteRatingAsync(ratingId);

            _ratingRepositoryMock.Verify(repo => repo.DeleteAsync(ratingId), Times.Once);

            _ratingRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
