using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;

        public RuleNameController(IRuleNameService ruleNameService)
        {
            _ruleNameService = ruleNameService;
        }


        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetRuleNameById(int id)
        {
            var ruleName = await _ruleNameService.GetRuleNameByIdAsync(id);

            if (ruleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            var ruleNameDTO = await _ruleNameService.GetRuleNameDTOByIdAsync(id);
            return Ok(ruleNameDTO);
        }


        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllRuleName()
        {
            var ruleNameDTOs = await _ruleNameService.GetAllRuleNameDTOsAsync();
            return Ok(ruleNameDTOs);
        }


        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateRuleName([FromBody] RuleNameDTO ruleNameDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _ruleNameService.CreateRuleNameAsync(ruleNameDTO);
            return Ok();
        }


        [HttpPut]
        [Route("api/update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleNameDTO updatedRuleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedRuleName.Id)
            {
                return BadRequest("RuleName ID mismatch.");
            }

            var existingRuleName = await _ruleNameService.GetRuleNameByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            await _ruleNameService.UpdateRuleNameAsync(updatedRuleName, existingRuleName);
            return Ok();
        }


        [HttpDelete]
        [Route("api/delete/{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            var existingRuleName = await _ruleNameService.GetRuleNameByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            await _ruleNameService.DeleteRuleNameAsync(id);
            return Ok();
        }
    }
}