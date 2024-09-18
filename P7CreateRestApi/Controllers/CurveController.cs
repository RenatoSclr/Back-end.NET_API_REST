using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;

        public CurveController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }



        [HttpGet]
        [Route("api/get/{id}")]
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


        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllCurvePoint()
        {
            var CurvePointDTOs = await _curvePointService.GetAllCurvePointDTOsAsync();
            return Ok(CurvePointDTOs);
        }


        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateCurvePoint([FromBody] CurvePointDTO CurvePointDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _curvePointService.CreateCurvePointAsync(CurvePointDTO);
            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePointDTO updatedCurvePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedCurvePoint.Id)
            {
                return BadRequest("CurvePoint ID mismatch.");
            }

            var existingCurvePoint = await _curvePointService.GetCurvePointByIdAsync(id);
            if (existingCurvePoint == null)
            {
                return NotFound($"CurvePoint with Id = {id} not found.");
            }

            await _curvePointService.UpdateCurvePointAsync(updatedCurvePoint, existingCurvePoint);
            return Ok();
        }


        [HttpDelete]
        [Route("api/delete/{id}")]
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