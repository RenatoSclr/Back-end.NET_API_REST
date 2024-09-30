using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.RatingDtos;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IRatingService
    {
        Task<List<ReadRatingDTO>> GetAllRatingDTOsAsync();

        Task CreateRatingAsync(EditRatingAdminDTO ratingDTO);

        Task<Rating> GetRatingByIdAsync(int id);

        Task<ReadRatingDTO> GetRatingDTOByIdAsync(int id);

        Task UpdateRatingAsync(EditRatingAdminDTO ratingDTO, Rating rating);

        Task DeleteRatingAsync(int id);
    }
}
