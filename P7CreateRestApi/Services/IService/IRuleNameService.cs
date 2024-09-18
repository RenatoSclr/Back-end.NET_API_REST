using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IRuleNameService
    {
        Task<List<RuleNameDTO>> GetAllRuleNameDTOsAsync();

        Task CreateRuleNameAsync(RuleNameDTO ruleNameDTO);

        Task<RuleName> GetRuleNameByIdAsync(int id);

        Task<RuleNameDTO> GetRuleNameDTOByIdAsync(int id);

        Task UpdateRuleNameAsync(RuleNameDTO ruleNameDTO, RuleName ruleName);

        Task DeleteRuleNameAsync(int id);
    }
}
