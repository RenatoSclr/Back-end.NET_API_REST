using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using P7CreateRestApi.Domain.DTO.CurvePointDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("curves")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;
        private readonly ILogger<CurveController> _logger; 

        public CurveController(ICurvePointService curvePointService, ILogger<CurveController> logger)
        {
            _curvePointService = curvePointService;
            _logger = logger;  
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin/{id}")]
        public async Task<IActionResult> GetCurvePointById(int id)
        {
            _logger.LogInformation("Fetching CurvePoint with Id {Id}", id);  

            var curvePoint = await _curvePointService.GetCurvePointByIdAsync(id);

            if (curvePoint == null)
            {
                _logger.LogWarning("CurvePoint with Id {Id} not found", id);  
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            var curvePointDTO = await _curvePointService.GetCurvePointDTOByIdAsync(id);
            _logger.LogInformation("CurvePoint with Id {Id} successfully fetched", id); 
            return Ok(curvePointDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAllCurvePointAsAdmin()
        {
            _logger.LogInformation("Fetching all CurvePoints for admin");
            var CurvePointDTOs = await _curvePointService.GetAllCurvePointDTOsAsAdminAsync();
            _logger.LogInformation("Successfully fetched all CurvePoints for admin");
            return Ok(CurvePointDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurvePointAsUser()
        {
            _logger.LogInformation("Fetching all CurvePoints for user");
            var CurvePointDTOs = await _curvePointService.GetAllCurvePointDTOsAsUserAsync();
            _logger.LogInformation("Successfully fetched all CurvePoints for user");
            return Ok(CurvePointDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> CreateCurvePoint([FromBody] CreateCurvePointAdminDTO CurvePointDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a CurvePoint");  
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new CurvePoint");
            await _curvePointService.CreateCurvePointAsAdminAsync(CurvePointDTO);
            _logger.LogInformation("CurvePoint successfully created");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("admin/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] UpdateCurvePointAdminDTO updatedCurvePoint)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating CurvePoint with Id {Id}", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating CurvePoint with Id {Id}", id);

            var existingCurvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (existingCurvePoint == null)
            {
                _logger.LogWarning("CurvePoint with Id {Id} not found for update", id);
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            await _curvePointService.UpdateCurvePointAsAdminAsync(updatedCurvePoint, existingCurvePoint);
            _logger.LogInformation("CurvePoint with Id {Id} successfully updated", id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("admin/{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            _logger.LogInformation("Deleting CurvePoint with Id {Id}", id);

            var existingCurvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (existingCurvePoint == null)
            {
                _logger.LogWarning("CurvePoint with Id {Id} not found for deletion", id);
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            await _curvePointService.DeleteCurvePointAsync(id);
            _logger.LogInformation("CurvePoint with Id {Id} successfully deleted", id);
            return Ok();
        }
    }
}
