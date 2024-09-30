using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using P7CreateRestApi.Domain.DTO.TradeDtos;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradeController> _logger; 

        public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _logger = logger; 
        }

        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetTradeAsUserById(int id)
        {
            _logger.LogInformation("Fetching Trade with Id {Id} for user", id);

            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                _logger.LogWarning("Trade with Id {Id} not found for user", id); 
                return NotFound($"Trade with Id = {id} not found.");
            }

            var tradeDTO = await _tradeService.GetTradeDTOAsUserByIdAsync(id);
            _logger.LogInformation("Successfully fetched Trade with Id {Id} for user", id); 
            return Ok(tradeDTO);
        }

        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllTradeAsUser()
        {
            _logger.LogInformation("User is fetching all trades");

            var tradeDTOs = await _tradeService.GetAllTradeDTOsAsUserAsync();

            _logger.LogInformation("Successfully fetched all trades for user"); 
            return Ok(tradeDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get/{id}")]
        public async Task<IActionResult> GetTradeAsAdminById(int id)
        {
            _logger.LogInformation("Admin is fetching Trade with Id {Id}", id); 

            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                _logger.LogWarning("Trade with Id {Id} not found for admin", id); 
                return NotFound($"Trade with Id = {id} not found.");
            }

            var tradeDTO = await _tradeService.GetTradeDTOAsAdminByIdAsync(id);
            _logger.LogInformation("Successfully fetched Trade with Id {Id} for admin", id); 
            return Ok(tradeDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get")]
        public async Task<IActionResult> GetAllTradeAsAdmin()
        {
            _logger.LogInformation("Admin is fetching all trades"); 

            var tradeDTOs = await _tradeService.GetAllTradeDTOsAsAdminAsync();

            _logger.LogInformation("Successfully fetched all trades for admin"); 
            return Ok(tradeDTOs);
        }

        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateTradeAsUser([FromBody] CreateTradeDTO tradeDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating trade by user"); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("User is creating a new Trade"); 
            await _tradeService.CreateTradeAsUserAsync(tradeDTO);
            _logger.LogInformation("Successfully created a new Trade by user"); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/admin/create")]
        public async Task<IActionResult> CreateTradeAsAdmin([FromBody] CreateTradeAdminDTO tradeDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating trade by admin"); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is creating a new Trade"); 
            await _tradeService.CreateTradeAsAdminAsync(tradeDTO);
            _logger.LogInformation("Successfully created a new Trade by admin"); 
            return Ok();
        }

        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateTradeAsUser(int id, [FromBody] UpdateTradeDTO updatedTrade)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating trade with Id {Id} by user", id); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("User is updating Trade with Id {Id}", id); 

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                _logger.LogWarning("Trade with Id {Id} not found for update by user", id);
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.UpdateTradeAsUserAsync(updatedTrade, existingTrade);
            _logger.LogInformation("Successfully updated Trade with Id {Id} by user", id); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateTradeAsAdmin(int id, [FromBody] UpdateTradeAdminDTO updatedTrade)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating trade with Id {Id} by admin", id); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is updating Trade with Id {Id}", id); 

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                _logger.LogWarning("Trade with Id {Id} not found for update by admin", id); 
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.UpdateTradeAsAdminAsync(updatedTrade, existingTrade);
            _logger.LogInformation("Successfully updated Trade with Id {Id} by admin", id); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/delete/{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            _logger.LogInformation("Admin is deleting Trade with Id {Id}", id); 

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                _logger.LogWarning("Trade with Id {Id} not found for deletion by admin", id); 
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.DeleteTradeAsync(id);
            _logger.LogInformation("Successfully deleted Trade with Id {Id} by admin", id); 
            return Ok();
        }
    }
}
