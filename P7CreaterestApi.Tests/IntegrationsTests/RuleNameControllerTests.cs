using System.Net;
using System.Net.Http.Json;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using P7CreateRestApi.Domain.DTO.RuleNameDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class RuleNameControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public RuleNameControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<LocalDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"InMemoryBidTestDatabase");
                    });

                });
            });

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private async Task ClearDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
                dbContext.RuleNames.RemoveRange(dbContext.RuleNames);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedSampleRuleNamesAsync()
        {
            var sampleRuleNames = GetSampleRuleNames();

            foreach (var ruleName in sampleRuleNames)
            {
                await _client.PostAsJsonAsync("/rulenames/admin", new EditRuleNameAdminDTO
                {
                    Name = ruleName.Name,
                    Description = ruleName.Description,
                    Json = ruleName.Json,
                    Template = ruleName.Template,
                    SqlStr = ruleName.SqlStr,
                    SqlPart = ruleName.SqlPart
                });
            }
        }

        private List<RuleName> GetSampleRuleNames()
        {
            return new List<RuleName>
            {
                new RuleName
                {
                    Name = "Rule1",
                    Description = "Rule Description 1",
                    Json = "Some Json",
                    Template = "Template 1",
                    SqlStr = "SELECT * FROM table1",
                    SqlPart = "WHERE condition1"
                },
                new RuleName
                {
                    Name = "Rule2",
                    Description = "Rule Description 2",
                    Json = "Some Json",
                    Template = "Template 2",
                    SqlStr = "SELECT * FROM table2",
                    SqlPart = "WHERE condition2"
                }
            };
        }

        [Fact]
        public async Task GetRuleNameById_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();

            var allRuleNamesResponse = await _client.GetAsync("/rulenames/admin");
            var ruleNames = await allRuleNamesResponse.Content.ReadFromJsonAsync<List<ReadRuleNameAdminDTO>>();
            var ruleNameToGet = ruleNames[0];

            // Act
            var response = await _client.GetAsync($"/rulenames/admin/{ruleNameToGet.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var ruleName = await response.Content.ReadFromJsonAsync<ReadRuleNameAdminDTO>();
            Assert.NotNull(ruleName);
            Assert.Equal(ruleNameToGet.Id, ruleName.Id);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetRuleNameById_AsUser_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();
            await _factory.AuthenticateUserAsync(_client);

            var allRuleNamesResponse = await _client.GetAsync("/rulenames");
            var ruleNames = await allRuleNamesResponse.Content.ReadFromJsonAsync<List<ReadRuleNameDTO>>();
            var ruleNameToGet = ruleNames[0];

            // Act
            var response = await _client.GetAsync($"/rulenames/{ruleNameToGet.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var ruleName = await response.Content.ReadFromJsonAsync<ReadRuleNameDTO>();
            Assert.NotNull(ruleName);
            Assert.Equal(ruleNameToGet.Id, ruleName.Id);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetRuleNameById_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);
            var ruleNameId = 1;

            // Act
            var response = await _client.GetAsync($"/rulenames/admin/{ruleNameId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllRuleNames_AsAdmin_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();

            // Act
            var response = await _client.GetAsync("/rulenames/admin");

            // Assert
            response.EnsureSuccessStatusCode();
            var ruleNames = await response.Content.ReadFromJsonAsync<List<ReadRuleNameAdminDTO>>();
            Assert.NotNull(ruleNames);
            Assert.NotEmpty(ruleNames);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task GetAllRuleNames_AsUser_ShouldReturnOkWithData()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();
            await _factory.AuthenticateUserAsync(_client);
            // Act
            var response = await _client.GetAsync("/rulenames");

            // Assert
            response.EnsureSuccessStatusCode();
            var ruleNames = await response.Content.ReadFromJsonAsync<List<ReadRuleNameDTO>>();
            Assert.NotNull(ruleNames);
            Assert.NotEmpty(ruleNames);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateRuleName_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var newRuleName = new EditRuleNameAdminDTO
            {
                Name = "RuleName",
                Description = "New rule description",
                Json = "Some Json",
                Template = "New Template",
                SqlStr = "SELECT * FROM table",
                SqlPart = "WHERE condition"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/rulenames/admin", newRuleName);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task CreateRuleName_AsUser_ShouldReturnForbidden()
        {
            // Arrange
            await _factory.AuthenticateUserAsync(_client);

            var newRuleName = new EditRuleNameAdminDTO
            {
                Name = "RuleName",
                Description = "New rule description",
                Json = "Some Json",
                Template = "New Template",
                SqlStr = "SELECT * FROM table",
                SqlPart = "WHERE condition"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/rulenames/admin", newRuleName);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateRuleName_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();

            var allRuleNamesResponse = await _client.GetAsync("/rulenames/admin");
            var ruleNames = await allRuleNamesResponse.Content.ReadFromJsonAsync<List<ReadRuleNameAdminDTO>>();
            var ruleNameToUpdate = ruleNames[0];

            var updatedRuleName = new EditRuleNameAdminDTO
            {
                Name = "Updated RuleName",
                Description = "Updated description",
                Json = "Some Json",
                Template = "Updated Template",
                SqlStr = "SELECT * FROM updated_table",
                SqlPart = "WHERE updated_condition"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/rulenames/admin/{ruleNameToUpdate.Id}", updatedRuleName);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedRuleNameResponse = await _client.GetAsync($"/rulenames/admin/{ruleNameToUpdate.Id}");
            var updatedRuleNameData = await updatedRuleNameResponse.Content.ReadFromJsonAsync<ReadRuleNameAdminDTO>();
            Assert.Equal(updatedRuleName.Name, updatedRuleNameData.Name);
            Assert.Equal(updatedRuleName.Description, updatedRuleNameData.Description);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task UpdateRuleName_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            var updatedRuleName = new EditRuleNameAdminDTO
            {
                Name = "Updated RuleName",
                Description = "Updated description",
                Json = "Some Json",
                Template = "Updated Template",
                SqlStr = "SELECT * FROM updated_table",
                SqlPart = "WHERE updated_condition"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/rulenames/admin/999999", updatedRuleName);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteRuleName_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);
            await SeedSampleRuleNamesAsync();

            var allRuleNamesResponse = await _client.GetAsync("/rulenames/admin");
            var ruleNames = await allRuleNamesResponse.Content.ReadFromJsonAsync<List<ReadRuleNameAdminDTO>>();
            var ruleNameToDelete = ruleNames[0];

            // Act
            var deleteResponse = await _client.DeleteAsync($"/rulenames/admin/{ruleNameToDelete.Id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var deletedRuleNameResponse = await _client.GetAsync($"/rulenames/admin/{ruleNameToDelete.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deletedRuleNameResponse.StatusCode);

            //Dispose
            await ClearDatabase();
        }

        [Fact]
        public async Task DeleteRuleName_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            await _factory.AuthenticateAdminAsync(_client);

            // Act
            var response = await _client.DeleteAsync($"/rulenames/admin/999999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            //Dispose
            await ClearDatabase();
        }
    }
}
