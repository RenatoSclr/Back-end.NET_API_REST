using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IRatingService
    {
        Task<List<RatingDTO>> GetAllRatingDTOsAsync();

        Task CreateRatingAsync(RatingDTO ratingDTO);

        Task<Rating> GetRatingByIdAsync(int id);

        Task<RatingDTO> GetRatingDTOByIdAsync(int id);

        Task UpdateRatingAsync(RatingDTO ratingDTO, Rating rating);

        Task DeleteRatingAsync(int id);
    }
}
