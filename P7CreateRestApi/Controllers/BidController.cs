using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.Domain.DTO.BidDtos;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("bids")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidService bidService, ILogger<BidController> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBidAsUserById(int id)
        {
            _logger.LogInformation("User requested Bid with Id {Id}", id);

            var bid = await _bidService.GetBidByIdAsync(id);

            if (bid == null)
            {
                _logger.LogWarning("Bid with Id {Id} not found", id);
                return NotFound($"Bid with Id = {id} not found.");
            }

            var bidDTO = await _bidService.GetBidDTOByIdAsync(id);
            _logger.LogInformation("Successfully fetched Bid with Id {Id} for user", id);
            return Ok(bidDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBidAsUser()
        {
            _logger.LogInformation("User requested all Bids");

            var bidDTOs = await _bidService.GetAllBidDTOsAsUserAsync();

            _logger.LogInformation("Successfully fetched all Bids for user");
            return Ok(bidDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetBidAsAdminById(int id)
        {
            _logger.LogInformation("Admin requested Bid with Id {Id}", id);

            var bid = await _bidService.GetBidByIdAsync(id);

            if (bid == null)
            {
                _logger.LogWarning("Bid with Id {Id} not found", id);
                return NotFound($"Bid with Id = {id} not found.");
            }

            var bidDTO = await _bidService.GetBidAdminDTOByIdAsync(id);
            _logger.LogInformation("Successfully fetched Bid with Id {Id} for admin", id);
            return Ok(bidDTO);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllBidAsAdmin()
        {
            _logger.LogInformation("Admin requested all Bids");

            var bidDTOs = await _bidService.GetAllBidDTOsAsAdminAsync();

            _logger.LogInformation("Successfully fetched all Bids for admin");
            return Ok(bidDTOs);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBidAsUser([FromBody] CreateBidDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a Bid by user");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("User is creating a new Bid");

            await _bidService.CreateBidAsUserAsync(bidDTO);

            _logger.LogInformation("Successfully created a new Bid by user");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateBidAsAdmin([FromBody] CreateBidAdminDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a Bid by admin");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is creating a new Bid");

            await _bidService.CreateBidAsAdminAsync(bidDTO);

            _logger.LogInformation("Successfully created a new Bid by admin");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBidAsUser(int id, [FromBody] UpdateBidDTO updatedBid)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating a Bid by user with Id {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("User is updating Bid with Id {Id}", id);

            var existingBid = await _bidService.GetBidByIdAsync(id);
            if (existingBid == null)
            {
                _logger.LogWarning("Bid with Id {Id} not found for update by user", id);
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.UpdateBidAsUserAsync(updatedBid, existingBid);

            _logger.LogInformation("Successfully updated Bid with Id {Id} by user", id);
            return Ok(updatedBid);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] UpdateBidAdminDTO updatedBid)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating a Bid by admin with Id {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is updating Bid with Id {Id}", id);

            var existingBid = await _bidService.GetBidByIdAsync(id);
            if (existingBid == null)
            {
                _logger.LogWarning("Bid with Id {Id} not found for update by admin", id);
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.UpdateBidAsAdminAsync(updatedBid, existingBid);

            _logger.LogInformation("Successfully updated Bid with Id {Id} by admin", id);
            return Ok(updatedBid);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            _logger.LogInformation("Admin is deleting Bid with Id {Id}", id);

            var existingBid = await _bidService.GetBidByIdAsync(id);
            if (existingBid == null)
            {
                _logger.LogWarning("Bid with Id {Id} not found for deletion by admin", id);
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.DeleteBidAsync(id);

            _logger.LogInformation("Successfully deleted Bid with Id {Id} by admin", id);
            return Ok();
        }
    }
}
