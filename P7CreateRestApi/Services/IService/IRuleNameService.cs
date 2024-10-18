using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;

namespace Dot.Net.WebApi.Services.IService
{
    public interface IRuleNameService
    {
        Task<List<ReadRuleNameAdminDTO>> GetAllRuleNameDTOsAsAdminAsync();
        Task<List<ReadRuleNameDTO>> GetAllRuleNameDTOsAsUserAsync();

        Task CreateRuleNameAsAdminAsync(EditRuleNameAdminDTO ruleNameDTO);

        Task<RuleName> GetRuleNameByIdAsync(int id);

        Task<ReadRuleNameAdminDTO> GetRuleNameDTOAsAdminByIdAsync(int id);
        Task<ReadRuleNameDTO> GetRuleNameDTOAsUserByIdAsync(int id);

        Task UpdateRuleNameAsAdminAsync(EditRuleNameAdminDTO ruleNameDTO, RuleName ruleName);

        Task DeleteRuleNameAsync(int id);
    }
}
