using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO.RatingDtos;

namespace Dot.Net.WebApi.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<List<ReadRatingDTO>> GetAllRatingDTOsAsync()
        {
            var ratingList = await _ratingRepository.GetAllAsync();
            return MapToRatingDTOList(ratingList.ToList());
        }

        public async Task CreateRatingAsync(EditRatingAdminDTO ratingDTO)
        {
            var rating = MapToRating(ratingDTO);
            await _ratingRepository.AddAsync(rating);
            await _ratingRepository.SaveAsync();
        }

        public async Task UpdateRatingAsync(EditRatingAdminDTO ratingDTO, Rating existingRating)
        {
            var rating = MapToRating(ratingDTO, existingRating);
            await _ratingRepository.UpdateAsync(rating);
            await _ratingRepository.SaveAsync();
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            return await _ratingRepository.GetByIdAsync(id);
        }


        public async Task<ReadRatingDTO> GetRatingDTOByIdAsync(int id)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            return MapToRatingDTO(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            await _ratingRepository.DeleteAsync(id);
            await _ratingRepository.SaveAsync();
        }


        private Rating MapToRating(EditRatingAdminDTO ratingDTO, Rating existingRating = null)
        {
            var rating = existingRating ?? new Rating();
            
            rating.MoodysRating = ratingDTO.MoodysRating;
            rating.OrderNumber = ratingDTO.OrderNumber;
            rating.SandPRating = ratingDTO.SandPRating;
            rating.FitchRating = ratingDTO.FitchRating;
            return rating;
        }

        private ReadRatingDTO MapToRatingDTO(Rating rating)
        {
            return new ReadRatingDTO
            {
                Id = rating.Id,
                FitchRating = rating.FitchRating,
                MoodysRating = rating.MoodysRating,
                OrderNumber = rating.OrderNumber,
                SandPRating = rating.SandPRating,
            };
        }

        private List<ReadRatingDTO> MapToRatingDTOList(List<Rating> ratingList)
        {
            return ratingList.Select(rating => new ReadRatingDTO
            {
                Id = rating.Id,
                FitchRating = rating.FitchRating,
                MoodysRating = rating.MoodysRating,
                OrderNumber = rating.OrderNumber,
                SandPRating = rating.SandPRating,
            }).ToList();
        }
    }
}
