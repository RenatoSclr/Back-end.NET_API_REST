using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Controllers
{
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
        public async Task<IActionResult> GetTradeById(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            var tradeDTO = await _tradeService.GetTradeDTOByIdAsync(id);
            return Ok(tradeDTO);
        }


        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllTrade()
        {
            var tradeDTOs = await _tradeService.GetAllTradeDTOsAsync();
            return Ok(tradeDTOs);
        }


        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateTrade([FromBody] TradeDTO tradeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeService.CreateTradeAsync(tradeDTO);
            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateTrade(int id, [FromBody] TradeDTO updatedTrade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedTrade.TradeId)
            {
                return BadRequest("Trade ID mismatch.");
            }

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with Id = {id} not found.");
            }

            await _tradeService.UpdateTradeAsync(updatedTrade, existingTrade);
            return Ok();
        }


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