using Dot.Net.WebApi.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("rulenames")]
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;
        private readonly ILogger<RuleNameController> _logger; 

        public RuleNameController(IRuleNameService ruleNameService, ILogger<RuleNameController> logger)
        {
            _ruleNameService = ruleNameService;
            _logger = logger; 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin/{id}")]
        public async Task<IActionResult> GetRuleNameAsAdminById(int id)
        {
            _logger.LogInformation("Admin is fetching RuleName with Id {Id}", id); 

            var ruleName = await _ruleNameService.GetRuleNameByIdAsync(id);

            if (ruleName == null)
            {
                _logger.LogWarning("RuleName with Id {Id} not found for admin", id);
                return NotFound();
            }

            var ruleNameDTO = await _ruleNameService.GetRuleNameDTOAsAdminByIdAsync(id);
            _logger.LogInformation("Successfully fetched RuleName with Id {Id} for admin", id); 
            return Ok(ruleNameDTO);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRuleNameAsUserById(int id)
        {
            _logger.LogInformation("User is fetching RuleName with Id {Id}", id); 

            var ruleName = await _ruleNameService.GetRuleNameByIdAsync(id);

            if (ruleName == null)
            {
                _logger.LogWarning("RuleName with Id {Id} not found for user", id);
                return NotFound($"RuleName with Id = {id} not found.");
            }

            var ruleNameDTO = await _ruleNameService.GetRuleNameDTOAsUserByIdAsync(id);
            _logger.LogInformation("Successfully fetched RuleName with Id {Id} for user", id); 
            return Ok(ruleNameDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAllRuleNamesAdmin()
        {
            _logger.LogInformation("Admin is fetching all RuleNames"); 

            var ruleNameDTOs = await _ruleNameService.GetAllRuleNameDTOsAsAdminAsync();
            _logger.LogInformation("Successfully fetched all RuleNames for admin"); 
            return Ok(ruleNameDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRuleNameAsUser()
        {
            _logger.LogInformation("User is fetching all RuleNames"); 

            var ruleNameDTOs = await _ruleNameService.GetAllRuleNameDTOsAsUserAsync();
            _logger.LogInformation("Successfully fetched all RuleNames for user"); 
            return Ok(ruleNameDTOs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> CreateRuleName([FromBody] EditRuleNameAdminDTO ruleNameDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state when creating RuleName"); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is creating a new RuleName"); 
            await _ruleNameService.CreateRuleNameAsAdminAsync(ruleNameDTO);
            _logger.LogInformation("Successfully created a new RuleName"); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("admin/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] EditRuleNameAdminDTO updatedRuleName)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state when updating RuleName with Id {Id}", id); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin is updating RuleName with Id {Id}", id); 

            var existingRuleName = await _ruleNameService.GetRuleNameByIdAsync(id);
            if (existingRuleName == null)
            {
                _logger.LogWarning("RuleName with Id {Id} not found for update", id); 
                return NotFound($"RuleName with Id = {id} not found.");
            }

            await _ruleNameService.UpdateRuleNameAsAdminAsync(updatedRuleName, existingRuleName);
            _logger.LogInformation("Successfully updated RuleName with Id {Id}", id); 
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("admin/{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            _logger.LogInformation("Admin is deleting RuleName with Id {Id}", id); 

            var existingRuleName = await _ruleNameService.GetRuleNameByIdAsync(id);
            if (existingRuleName == null)
            {
                _logger.LogWarning("RuleName with Id {Id} not found for deletion", id); 
                return NotFound($"RuleName with Id = {id} not found.");
            }

            await _ruleNameService.DeleteRuleNameAsync(id);
            _logger.LogInformation("Successfully deleted RuleName with Id {Id}", id); 
            return Ok();
        }
    }
}