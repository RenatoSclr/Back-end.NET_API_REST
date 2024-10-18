using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.RatingDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ratings")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IRatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger; 
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            _logger.LogInformation("Fetching Rating with Id {Id}", id); 

            var rating = await _ratingService.GetRatingByIdAsync(id);

            if (rating == null)
            {
                _logger.LogWarning("Rating with Id {Id} not found", id); 
                return NotFound($"Rating with Id = {id} not found.");
            }

            var ratingDTO = await _ratingService.GetRatingDTOByIdAsync(id);
            _logger.LogInformation("Successfully fetched Rating with Id {Id}", id); 
            return Ok(ratingDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRating()
        {
            _logger.LogInformation("Fetching all Ratings"); 

            var ratingDTOs = await _ratingService.GetAllRatingDTOsAsync();
            _logger.LogInformation("Successfully fetched all Ratings"); 
            return Ok(ratingDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> CreateRating([FromBody] EditRatingAdminDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating Rating"); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is creating a new Rating"); 
            await _ratingService.CreateRatingAsync(ratingDTO);
            _logger.LogInformation("Successfully created a new Rating"); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("admin/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] EditRatingAdminDTO updatedRating)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating Rating with Id {Id}", id); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is updating Rating with Id {Id}", id); 

            var existingRating = await _ratingService.GetRatingByIdAsync(id);
            if (existingRating == null)
            {
                _logger.LogWarning("Rating with Id {Id} not found for update", id); 
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _ratingService.UpdateRatingAsync(updatedRating, existingRating);
            _logger.LogInformation("Successfully updated Rating with Id {Id}", id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("admin/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            _logger.LogInformation("Admin is deleting Rating with Id {Id}", id); 

            var existingRating = await _ratingService.GetRatingByIdAsync(id);
            if (existingRating == null)
            {
                _logger.LogWarning("Rating with Id {Id} not found for deletion", id); 
                return NotFound($"Rating with Id = {id} not found.");
            }

            await _ratingService.DeleteRatingAsync(id);
            _logger.LogInformation("Successfully deleted Rating with Id {Id}", id); 
            return Ok();
        }
    }
}
