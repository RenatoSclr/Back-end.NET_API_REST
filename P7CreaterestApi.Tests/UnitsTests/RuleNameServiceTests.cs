using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services;
using Moq;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class RuleNameServiceTests
    {
        private readonly Mock<IRuleNameRepository> _ruleNameRepositoryMock;
        private readonly RuleNameService _ruleNameService;

        public RuleNameServiceTests()
        {
            _ruleNameRepositoryMock = new Mock<IRuleNameRepository>();
            _ruleNameService = new RuleNameService(_ruleNameRepositoryMock.Object);
        }

        private List<RuleName> GetListRuleNameEntity()
        {
            return new List<RuleName>
            {
                new RuleName
                {
                    Id = 1,
                    Name = "Rule 1",
                    Description = "Description for Rule 1",
                    Json = "{ \"key\": \"value\" }",
                    Template = "Template 1",
                    SqlStr = "SELECT * FROM table1",
                    SqlPart = "WHERE id = 1"
                },
                new RuleName
                {
                    Id = 2,
                    Name = "Rule 2",
                    Description = "Description for Rule 2",
                    Json = "{ \"key\": \"value2\" }",
                    Template = "Template 2",
                    SqlStr = "SELECT * FROM table2",
                    SqlPart = "WHERE id = 2"
                },
                new RuleName
                {
                    Id = 3,
                    Name = "Rule 3",
                    Description = "Description for Rule 3",
                    Json = "{ \"key\": \"value3\" }",
                    Template = "Template 3",
                    SqlStr = "SELECT * FROM table3",
                    SqlPart = "WHERE id = 3"
                }
            };
        }

        [Fact]
        public async Task GetAllRuleNameDTOsAsAdminAsync_ShouldReturnRuleNameDTOList_WhenRuleNamesExist()
        {
            // Arrange
            _ruleNameRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListRuleNameEntity());

            // Act
            var result = await _ruleNameService.GetAllRuleNameDTOsAsAdminAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Rule 1", result[0].Name);
            Assert.Equal("Rule 2", result[1].Name);
            Assert.Equal("Rule 3", result[2].Name);
        }

        [Fact]
        public async Task GetAllRuleNameDTOsAsUserAsync_ShouldReturnRuleNameDTOList_WhenRuleNamesExist()
        {
            // Arrange
            _ruleNameRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(GetListRuleNameEntity());

            // Act
            var result = await _ruleNameService.GetAllRuleNameDTOsAsUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Rule 1", result[0].Name);
            Assert.Equal("Rule 2", result[1].Name);
            Assert.Equal("Rule 3", result[2].Name);
        }

        [Fact]
        public async Task CreateRuleNameAsAdmin_ShouldAddAndSaveRuleName()
        {
            // Arrange
            var editRuleNameAdminDTO = new EditRuleNameAdminDTO
            {
                Name = "New Rule",
                Description = "New Description",
                Json = "{ \"key\": \"new_value\" }",
                Template = "New Template",
                SqlStr = "SELECT * FROM new_table",
                SqlPart = "WHERE id = 10"
            };

            // Act
            await _ruleNameService.CreateRuleNameAsAdminAsync(editRuleNameAdminDTO);

            // Assert
            _ruleNameRepositoryMock.Verify(repo => repo.AddAsync(It.Is<RuleName>(rn =>
                rn.Name == editRuleNameAdminDTO.Name &&
                rn.Description == editRuleNameAdminDTO.Description &&
                rn.Json == editRuleNameAdminDTO.Json &&
                rn.Template == editRuleNameAdminDTO.Template &&
                rn.SqlStr == editRuleNameAdminDTO.SqlStr &&
                rn.SqlPart == editRuleNameAdminDTO.SqlPart
            )), Times.Once);

            _ruleNameRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateRuleNameAsAdmin_ShouldUpdateAndSaveRuleName()
        {
            // Arrange
            var editRuleNameAdminDTO = new EditRuleNameAdminDTO
            {
                Name = "Updated Rule",
                Description = "Updated Description",
                Json = "{ \"key\": \"updated_value\" }",
                Template = "Updated Template",
                SqlStr = "SELECT * FROM updated_table",
                SqlPart = "WHERE id = 20"
            };

            var existingRuleName = GetListRuleNameEntity()[0];

            // Act
            await _ruleNameService.UpdateRuleNameAsAdminAsync(editRuleNameAdminDTO, existingRuleName);

            // Assert
            _ruleNameRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<RuleName>(rn =>
                rn.Name == editRuleNameAdminDTO.Name &&
                rn.Description == editRuleNameAdminDTO.Description &&
                rn.Json == editRuleNameAdminDTO.Json &&
                rn.Template == editRuleNameAdminDTO.Template &&
                rn.SqlStr == editRuleNameAdminDTO.SqlStr &&
                rn.SqlPart == editRuleNameAdminDTO.SqlPart
            )), Times.Once);

            _ruleNameRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetRuleNameById_ShouldReturnRuleName_WhenIdIsValid()
        {
            var id = 1;

            _ruleNameRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListRuleNameEntity()[0]);

            var result = await _ruleNameService.GetRuleNameByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Rule 1", result.Name);
            Assert.Equal("{ \"key\": \"value\" }", result.Json);
        }

        [Fact]
        public async Task GetRuleNameDTOAsAdminById_ShouldReturnRuleNameDTO_WhenIdIsValid()
        {
            var id = 1;

            _ruleNameRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListRuleNameEntity()[0]);

            var result = await _ruleNameService.GetRuleNameDTOAsAdminByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Rule 1", result.Name);
            Assert.Equal("{ \"key\": \"value\" }", result.Json);
        }

        [Fact]
        public async Task GetRuleNameDTOAsUserById_ShouldReturnRuleNameDTO_WhenIdIsValid()
        {
            var id = 1;

            _ruleNameRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(GetListRuleNameEntity()[0]);

            var result = await _ruleNameService.GetRuleNameDTOAsUserByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Rule 1", result.Name);
            Assert.Equal("Description for Rule 1", result.Description);
        }

        [Fact]
        public async Task DeleteRuleNameAsync_ShouldCallDeleteAndSave_WhenIdIsValid()
        {
            int ruleNameId = 1;

            await _ruleNameService.DeleteRuleNameAsync(ruleNameId);

            _ruleNameRepositoryMock.Verify(repo => repo.DeleteAsync(ruleNameId), Times.Once);

            _ruleNameRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
