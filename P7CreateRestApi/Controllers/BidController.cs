using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

      
        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetBidById(int id)
        {
            var bid = await _bidService.GetBidByIdAsync(id);  

            if (bid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            var bidDTO = await _bidService.GetBidDTOByIdAsync(id);
            return Ok(bidDTO);
        }

     
        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllBid()
        {
            var bidDTOs = await _bidService.GetAllBidDTOsAsync();  
            return Ok(bidDTOs);
        }

       
        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateBid([FromBody] BidDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bidService.CreateBidAsync(bidDTO);  
            return Ok();
        }

  
        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidDTO updatedBid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedBid.BidDTOId)
            {
                return BadRequest("Bid ID mismatch.");
            }

            var existingBid = await _bidService.GetBidByIdAsync(id);  
            if (existingBid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.UpdateBidAsync(updatedBid, existingBid);  
            return Ok();
        }

      
        [HttpDelete]
        [Route("api/delete/{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var existingBid = await _bidService.GetBidByIdAsync(id); 
            if (existingBid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.DeleteBidAsync(id);  
            return Ok();
        }
    }
}
