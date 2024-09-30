using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;

        public CurveController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get/{id}")]
        public async Task<IActionResult> GetCurvePointById(int id)
        {
            var curvePoint = await _curvePointService.GetCurvePointByIdAsync(id);

            if (curvePoint == null)
            {
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            var curvePointDTO = await _curvePointService.GetCurvePointDTOByIdAsync(id);
            return Ok(curvePointDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get")]
        public async Task<IActionResult> GetAllCurvePointAsAdmin()
        {
            var CurvePointDTOs = await _curvePointService.GetAllCurvePointDTOsAsAdminAsync();
            return Ok(CurvePointDTOs);
        }

        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllCurvePointAsUser()
        {
            var CurvePointDTOs = await _curvePointService.GetAllCurvePointDTOsAsUserAsync();
            return Ok(CurvePointDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/admin/create")]
        public async Task<IActionResult> CreateCurvePoint([FromBody] CreateCurvePointAdminDTO CurvePointDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _curvePointService.CreateCurvePointAsAdminAsync(CurvePointDTO);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] UpdateCurvePointAdminDTO updatedCurvePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCurvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (existingCurvePoint == null)
            {
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            await _curvePointService.UpdateCurvePointAsAdminAsync(updatedCurvePoint, existingCurvePoint);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/admin/delete/{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            var existingCurvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (existingCurvePoint == null)
            {
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            await _curvePointService.DeleteCurvePointAsync(id);
            return Ok();
        }
    }
}