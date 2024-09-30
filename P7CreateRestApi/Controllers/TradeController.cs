using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.TradeDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }


        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetTradeAsUserById(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            var tradeDTO = await _tradeService.GetTradeDTOAsUserByIdAsync(id);
            return Ok(tradeDTO);
        }


        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllTradeAsUser()
        {
            var tradeDTOs = await _tradeService.GetAllTradeDTOsAsUserAsync();
            return Ok(tradeDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get/{id}")]
        public async Task<IActionResult> GetTradeAsAdminById(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            var tradeDTO = await _tradeService.GetTradeDTOAsAdminByIdAsync(id);
            return Ok(tradeDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get")]
        public async Task<IActionResult> GetAllTradeAsAdmin()
        {
            var tradeDTOs = await _tradeService.GetAllTradeDTOsAsAdminAsync();
            return Ok(tradeDTOs);
        }

        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateTradeAsUser([FromBody] CreateTradeDTO tradeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeService.CreateTradeAsUserAsync(tradeDTO);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/admin/create")]
        public async Task<IActionResult> CreateTradeAsAdmin([FromBody] CreateTradeAdminDTO tradeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeService.CreateTradeAsAdminAsync(tradeDTO);
            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateTradeAsUser(int id, [FromBody] UpdateTradeDTO updatedTrade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.UpdateTradeAsUserAsync(updatedTrade, existingTrade);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateTradeAsAdmin(int id, [FromBody] UpdateTradeAdminDTO updatedTrade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.UpdateTradeAsAdminAsync(updatedTrade, existingTrade);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/delete/{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.DeleteTradeAsync(id);
            return Ok();
        }
    }
}