using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.RatingDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }


        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _ratingService.GetRatingByIdAsync(id);

            if (rating == null)
            {
                return NotFound($"Rating with Id = {id} not found.");
            }

            var ratingDTO = await _ratingService.GetRatingDTOByIdAsync(id);
            return Ok(ratingDTO);
        }


        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllRating()
        {
            var ratingDTOs = await _ratingService.GetAllRatingDTOsAsync();
            return Ok(ratingDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/admin/create")]
        public async Task<IActionResult> CreateRating([FromBody] EditRatingAdminDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _ratingService.CreateRatingAsync(ratingDTO);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] EditRatingAdminDTO updatedRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRating = await _ratingService.GetRatingByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _ratingService.UpdateRatingAsync(updatedRating, existingRating);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/admin/delete/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var existingRating = await _ratingService.GetRatingByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound($"Rating with Id = {id} not found.");
            }

            await _ratingService.DeleteRatingAsync(id);
            return Ok();
        }
    }
}