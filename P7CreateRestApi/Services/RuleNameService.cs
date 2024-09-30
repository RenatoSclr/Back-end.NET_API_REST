using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;

namespace Dot.Net.WebApi.Services
{
    public class RuleNameService : IRuleNameService
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task<List<ReadRuleNameAdminDTO>> GetAllRuleNameDTOsAsAdminAsync()
        {
            var ruleNameList = await _ruleNameRepository.GetAllAsync();
            return MapToRuleNameAdminDTOList(ruleNameList.ToList());
        }

        public async Task<List<ReadRuleNameDTO>> GetAllRuleNameDTOsAsUserAsync()
        {
            var ruleNameList = await _ruleNameRepository.GetAllAsync();
            return MapToRuleNameUserDTOList(ruleNameList.ToList());
        }

        public async Task CreateRuleNameAsAdminAsync(EditRuleNameAdminDTO ruleNameDTO)
        {
            var ruleName = MapToRuleName(ruleNameDTO);
            await _ruleNameRepository.AddAsync(ruleName);
            await _ruleNameRepository.SaveAsync();
        }

        public async Task UpdateRuleNameAsAdminAsync(EditRuleNameAdminDTO ruleNameDTO, RuleName existingRuleName)
        {
            var ruleName = MapToRuleName(ruleNameDTO, existingRuleName);
            await _ruleNameRepository.UpdateAsync(ruleName);
            await _ruleNameRepository.SaveAsync();
        }

        public async Task<RuleName> GetRuleNameByIdAsync(int id)
        {
            return await _ruleNameRepository.GetByIdAsync(id);
        }


        public async Task<ReadRuleNameAdminDTO> GetRuleNameDTOAsAdminByIdAsync(int id)
        {
            var ruleName = await GetRuleNameByIdAsync(id);
            return MapToRuleNameAdminDTO(ruleName);
        }

        public async Task<ReadRuleNameDTO> GetRuleNameDTOAsUserByIdAsync(int id)
        {
            var ruleName = await GetRuleNameByIdAsync(id);
            return MapToRuleNameUserDTO(ruleName);
        }

        public async Task DeleteRuleNameAsync(int id)
        {
            await _ruleNameRepository.DeleteAsync(id);
            await _ruleNameRepository.SaveAsync();
        }


        private RuleName MapToRuleName(EditRuleNameAdminDTO ruleNameDTO, RuleName existingRuleName = null)
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

        private ReadRuleNameAdminDTO MapToRuleNameAdminDTO(RuleName ruleName)
        {
            return new ReadRuleNameAdminDTO
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

        private ReadRuleNameDTO MapToRuleNameUserDTO(RuleName ruleName)
        {
            return new ReadRuleNameDTO
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description
            };
        }

        private List<ReadRuleNameAdminDTO> MapToRuleNameAdminDTOList(List<RuleName> ruleNameList)
        {
            return ruleNameList.Select(ruleName => new ReadRuleNameAdminDTO
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

        private List<ReadRuleNameDTO> MapToRuleNameUserDTOList(List<RuleName> ruleNameList)
        {
            return ruleNameList.Select(ruleName => new ReadRuleNameDTO
            {
                Id = ruleName.Id,
                Name = ruleName.Name,
                Description = ruleName.Description
            }).ToList();
        }
    }
}
