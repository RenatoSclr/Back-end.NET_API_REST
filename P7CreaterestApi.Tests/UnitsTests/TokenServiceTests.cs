using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using P7CreateRestApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class TokenServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();
            _tokenService = new TokenService(_userManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = "123",
                UserName = "testuser"
            };

            var roles = new List<string> { "Admin", "User" };

            _userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(roles);

            _configurationMock.Setup(config => config["Jwt:SecretKey"])
                .Returns("ThisIsASecretKeyForJwtToken");

            _configurationMock.Setup(config => config["Jwt:Issuer"])
                .Returns("TestIssuer");

            _configurationMock.Setup(config => config["Jwt:Audience"])
                .Returns("TestAudience");

            // Act
            var token = await _tokenService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;


            Assert.NotNull(jsonToken);
            Assert.Equal("TestIssuer", jsonToken.Issuer);
            Assert.Equal("TestAudience", jsonToken.Audiences.First());
            Assert.Equal("testuser", jsonToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Contains(jsonToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            Assert.Contains(jsonToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        }
    }
}
