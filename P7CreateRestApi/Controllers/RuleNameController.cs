using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;

        public RuleNameController(IRuleNameService ruleNameService)
        {
            _ruleNameService = ruleNameService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get/{id}")]
        public async Task<IActionResult> GetRuleNameAsAdminById(int id)
        {
            var ruleName = await _ruleNameService.GetRuleNameByIdAsync(id);

            if (ruleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            var ruleNameDTO = await _ruleNameService.GetRuleNameDTOAsAdminByIdAsync(id);
            return Ok(ruleNameDTO);
        }

        [HttpGet]
        [Route("api/get/{id}")]
        public async Task<IActionResult> GetRuleNameAsUserById(int id)
        {
            var ruleName = await _ruleNameService.GetRuleNameByIdAsync(id);

            if (ruleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            var ruleNameDTO = await _ruleNameService.GetRuleNameDTOAsUserByIdAsync(id);
            return Ok(ruleNameDTO);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/admin/get")]
        public async Task<IActionResult> GetAllRuleNamesAdmin()
        {
            var ruleNameDTOs = await _ruleNameService.GetAllRuleNameDTOsAsAdminAsync();
            return Ok(ruleNameDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/get")]
        public async Task<IActionResult> GetAllRuleNameAsUser()
        {
            var ruleNameDTOs = await _ruleNameService.GetAllRuleNameDTOsAsUserAsync();
            return Ok(ruleNameDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/create")]
        public async Task<IActionResult> CreateRuleName([FromBody] EditRuleNameAdminDTO ruleNameDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _ruleNameService.CreateRuleNameAsAdminAsync(ruleNameDTO);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/admin/update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] EditRuleNameAdminDTO updatedRuleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRuleName = await _ruleNameService.GetRuleNameByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with Id = {id} not found.");
            }

            await _ruleNameService.UpdateRuleNameAsAdminAsync(updatedRuleName, existingRuleName);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/admin/delete/{id}")]
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