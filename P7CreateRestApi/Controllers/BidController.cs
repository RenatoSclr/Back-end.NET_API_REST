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
        public IActionResult Get(BidDTO bidDTO)
        {
            // TODO: check data valid and save to db, after saving return bid list

            return Ok();
        }

        [HttpGet]
        [Route("api/get")]
        public IActionResult GetAll(List<BidDTO> bidDTO)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }

        [HttpPost]
        [Route("api/create")]
        public IActionResult CreateBid([FromBody] BidDTO bidDTO)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public IActionResult UpdateBid(int id, [FromBody] BidDTO bidDTO)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return Ok();
        }

        [HttpDelete]
        [Route("api/delete/{id}")]
        public IActionResult DeleteBid(int id)
        {
            return Ok();
        }
    }
}