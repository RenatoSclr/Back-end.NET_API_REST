using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;

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
        public IActionResult GetBidById(int id)
        {
            
            var bidDTO = _bidService.GetBidDTOById(id);
            if (bidDTO == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            return Ok(bidDTO);
        }

        [HttpGet]
        [Route("api/get")]
        public IActionResult GetAllBid()
        {
            var bidDTO = _bidService.GetAllBidDTOs();
            return Ok(bidDTO);
        }

        [HttpPost]
        [Route("api/create")]
        public IActionResult CreateBid([FromBody] BidDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

             _bidService.CreateBid(bidDTO);

            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public IActionResult UpdateBid(int id, [FromBody] BidDTO updatedBid)
        {
           if (!ModelState.IsValid) 
           {
                return BadRequest(ModelState);
           }

            if (id != updatedBid.BidDTOId)
            {
                return BadRequest("Product ID mismatch.");
            }


           var existingBid = _bidService.GetBidById(id);
           if (existingBid == null)
           {
                return NotFound($"Bid with Id = {id} not found.");
           }

           _bidService.UpdateBid(updatedBid, existingBid);

            return Ok();
        }

        [HttpDelete]
        [Route("api/delete/{id}")]
        public IActionResult DeleteBid(int id)
        {
            var existingBid = _bidService.GetBidById(id);
            if (existingBid == null) 
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            _bidService.DeleteBid(id);
            return Ok();
        }
    }
}