using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using P7CreateRestApi.Domain.DTO.UserDtos;
using P7CreateRestApi.Services;

namespace P7CreateRestApi.Tests.UnitsTests
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userManagerMock = MockUserManager();
            _roleManagerMock = MockRoleManager();
            _userService = new UserService(_userManagerMock.Object, _roleManagerMock.Object);
        }

        private static Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }

        private List<User> GetSampleUsers()
        {
            return new List<User>
            {
                new User { Id = "1", UserName = "User1", Email = "user1@test.com", FullName = "Test User1" },
                new User { Id = "2", UserName = "User2", Email = "user2@test.com", FullName = "Test User2" }
            };
        }

        [Fact]
        public async Task GetAllUsersForAdminAsync_ShouldReturnUserList_WhenUsersExist()
        {
            // Arrange
            var sampleUsers = GetSampleUsers();
            _userManagerMock.Setup(x => x.Users).Returns(sampleUsers.AsQueryable());
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin" });

            // Act
            var result = await _userService.GetAllUsersForAdminAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("User1", result[0].UserName);
            Assert.Equal("Admin", result[0].Roles[0]);
        }

        [Fact]
        public async Task GetUserAdminDTOByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = "1";
            var sampleUser = GetSampleUsers().First();
            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(sampleUser);
            _userManagerMock.Setup(x => x.GetRolesAsync(sampleUser)).ReturnsAsync(new List<string> { "Admin" });

            // Act
            var result = await _userService.GetUserAdminDTOByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User1", result.UserName);
            Assert.Equal("Admin", result.Roles[0]);
        }

        [Fact]
        public async Task CreateUserWithDefaultRoleAsync_ShouldCreateUserAndAddRole_WhenCalled()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                UserName = "NewUser",
                Email = "newuser@test.com",
                Password = "Password123"
            };

            var createdUser = new User
            {
                UserName = createUserDTO.UserName,
                Email = createUserDTO.Email
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), createUserDTO.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(false);
            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.CreateUserWithDefaultRoleAsync(createUserDTO);

            // Assert
            Assert.True(result.Succeeded);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsAdminAsync_ShouldCreateUserAndAddMultipleRoles_WhenCalled()
        {
            // Arrange
            var createUserAdminDTO = new CreateUserAdminDTO
            {
                UserName = "AdminUser",
                Email = "adminuser@test.com",
                Password = "Password123",
                Roles = new List<string> { "Admin", "Manager" }
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), createUserAdminDTO.Password))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _userService.CreateUserAsAdminAsync(createUserAdminDTO);

            // Assert
            Assert.True(result.Succeeded);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "Admin"), Times.Once);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), "Manager"), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAdminAsync_ShouldUpdateUserDetailsAndRoles_WhenCalled()
        {
            // Arrange
            var sampleUser = GetSampleUsers().First();
            var updateUserAdminDTO = new UpdateUserAdminDTO
            {
                UserName = "UpdatedUser",
                Email = "updated@test.com",
                Roles = new List<string> { "Admin" }
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(sampleUser.Id)).ReturnsAsync(sampleUser);
            _userManagerMock.Setup(x => x.GetRolesAsync(sampleUser)).ReturnsAsync(new List<string> { "User" });
            _userManagerMock.Setup(x => x.RemoveFromRolesAsync(sampleUser, It.IsAny<IList<string>>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRolesAsync(sampleUser, updateUserAdminDTO.Roles))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.UpdateAsync(sampleUser)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.UpdateUserAdminAsync(sampleUser, updateUserAdminDTO);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("UpdatedUser", sampleUser.UserName);
            _userManagerMock.Verify(x => x.RemoveFromRolesAsync(sampleUser, It.IsAny<IList<string>>()), Times.Once);
            _userManagerMock.Verify(x => x.AddToRolesAsync(sampleUser, updateUserAdminDTO.Roles), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser_WhenCalled()
        {
            // Arrange
            var sampleUser = GetSampleUsers().First();

            _userManagerMock.Setup(x => x.DeleteAsync(sampleUser)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.DeleteUserAsync(sampleUser);

            // Assert
            Assert.True(result.Succeeded);
            _userManagerMock.Verify(x => x.DeleteAsync(sampleUser), Times.Once);
        }
    }
}
