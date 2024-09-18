using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services
{
    public class RuleNameService : IRuleNameService
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task<List<RuleNameDTO>> GetAllRuleNameDTOsAsync()
        {
            var ruleNameList = await _ruleNameRepository.GetAllAsync();
            return MapToRuleNameDTOList(ruleNameList.ToList());
        }

        public async Task CreateRuleNameAsync(RuleNameDTO ruleNameDTO)
        {
            var ruleName = MapToRuleName(ruleNameDTO);
            await _ruleNameRepository.AddAsync(ruleName);
            await _ruleNameRepository.SaveAsync();
        }

        public async Task UpdateRuleNameAsync(RuleNameDTO ruleNameDTO, RuleName existingRuleName)
        {
            var ruleName = MapToRuleName(ruleNameDTO, existingRuleName);
            await _ruleNameRepository.UpdateAsync(ruleName);
            await _ruleNameRepository.SaveAsync();
        }

        public async Task<RuleName> GetRuleNameByIdAsync(int id)
        {
            return await _ruleNameRepository.GetByIdAsync(id);
        }


        public async Task<RuleNameDTO> GetRuleNameDTOByIdAsync(int id)
        {
            var ruleName = await _ruleNameRepository.GetByIdAsync(id);
            return MapToRuleNameDTO(ruleName);
        }

        public async Task DeleteRuleNameAsync(int id)
        {
            await _ruleNameRepository.DeleteAsync(id);
            await _ruleNameRepository.SaveAsync();
        }


        private RuleName MapToRuleName(RuleNameDTO ruleNameDTO, RuleName existingRuleName = null)
        {
            var ruleName = existingRuleName ?? new RuleName();
            
            ruleName.Name = ruleNameDTO.Name;
            ruleName.Description = ruleNameDTO.Description; 
            ruleName.Json = ruleNameDTO.Json;
            ruleName.Template = ruleNameDTO.Template;
            ruleName.SqlStr = ruleNameDTO.SqlStr;
            ruleName.SqlPart = ruleNameDTO.SqlPart;
            return ruleName;
        }

        private RuleNameDTO MapToRuleNameDTO(RuleName ruleName)
        {
            return new RuleNameDTO
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart
            };
        }

        private List<RuleNameDTO> MapToRuleNameDTOList(List<RuleName> ruleNameList)
        {
            return ruleNameList.Select(ruleName => new RuleNameDTO
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description,
                Json = ruleName.Json,
                Template = ruleName.Template,
                SqlStr = ruleName.SqlStr,
                SqlPart = ruleName.SqlPart
            }).ToList();
        }
    }
}
