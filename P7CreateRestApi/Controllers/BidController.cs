using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.BidDtos;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
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
        [Route("api/user/get/{id}")]
        public async Task<IActionResult> GetBidAsUserById(int id)
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
        [Route("api/user/get")]
        public async Task<IActionResult> GetAllBidAsUser()
        {
            var bidDTOs = await _bidService.GetAllBidDTOsAsUserAsync();
            return Ok(bidDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get/{id}")]
        public async Task<IActionResult> GetBidAsAdminById(int id)
        {
            var bid = await _bidService.GetBidByIdAsync(id);

            if (bid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            var bidDTO = await _bidService.GetBidAdminDTOByIdAsync(id);
            return Ok(bidDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get")]
        public async Task<IActionResult> GetAllBidAsAdmin()
        {
            var bidDTOs = await _bidService.GetAllBidDTOsAsAdminAsync();  
            return Ok(bidDTOs);
        }

        

        [HttpPost]
        [Route("api/user/create")]
        public async Task<IActionResult> CreateBidAsUser([FromBody] CreateBidDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bidService.CreateBidAsUserAsync(bidDTO);  
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/admin/create")]
        public async Task<IActionResult> CreateBidAsAdmin([FromBody] CreateBidAdminDTO bidDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bidService.CreateBidAsAdminAsync(bidDTO);
            return Ok();
        }


        [HttpPut]
        [Route("api/user/update/{id}")]
        public async Task<IActionResult> UpdateBidAsUser(int id, [FromBody] UpdateBidDTO updatedBid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBid = await _bidService.GetBidByIdAsync(id);  
            if (existingBid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.UpdateBidAsUserAsync(updatedBid, existingBid);  
            return Ok(updatedBid);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] UpdateBidAdminDTO updatedBid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBid = await _bidService.GetBidByIdAsync(id);
            if (existingBid == null)
            {
                return NotFound($"Bid with Id = {id} not found.");
            }

            await _bidService.UpdateBidAsAdminAsync(updatedBid, existingBid);
            return Ok(updatedBid);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/admin/delete/{id}")]
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
