using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Controllers.ControllersExtension;
using P7CreateRestApi.Domain.DTO.UserDtos;
using P7CreateRestApi.Services.IService;
using System.Security.Claims;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger; 

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {
            _logger.LogInformation("Attempting to register user {UserName}", createUserDTO.UserName);
            var result = await _userService.CreateUserWithDefaultRoleAsync(createUserDTO);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserName} successfully registered.", createUserDTO.UserName); 
                return Ok($"User {createUserDTO.UserName} created with role 'User'");
            }
            _logger.LogWarning("User registration failed for {UserName}. Errors: {Errors}", createUserDTO.UserName, result.Errors);

            result.ToModelResult(ModelState);

            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateUserAsAdmin([FromBody] CreateUserAdminDTO createUserDTO)
        {
            _logger.LogInformation("Admin creating a new user {UserName} with roles {Roles}", createUserDTO.UserName, createUserDTO.Roles); 
            var result = await _userService.CreateUserAsAdminAsync(createUserDTO);

            if (result.Succeeded)
            {
                _logger.LogInformation("Admin successfully created user {UserName} with roles {Roles}", createUserDTO.UserName, createUserDTO.Roles); 
                return Ok($"User {createUserDTO.UserName} created with role {createUserDTO.Roles}");
            }
            _logger.LogWarning("Admin failed to create user {UserName}. Errors: {Errors}", createUserDTO.UserName, result.Errors);

            result.ToModelResult(ModelState);
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("my-account")]
        public async Task<IActionResult> GetUserSelfData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access attempt. User ID not found."); 
                return Unauthorized("User ID not found.");
            }

            _logger.LogInformation("Fetching data for user ID {UserId}", userId); 
            var user = await _userService.GetUserDTOByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID {UserId}", userId); 
                return NotFound("User not found.");
            }

            _logger.LogInformation("Successfully fetched data for user ID {UserId}", userId); 
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Admin fetching all users."); 
            var users = await _userService.GetAllUsersForAdminAsync();
            _logger.LogInformation("Successfully fetched all users for admin."); 
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetUserAsAdminById(string id)
        {
            _logger.LogInformation("Admin fetching user by ID {Id}", id); 
            var user = await _userService.GetUserAdminDTOByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID {Id}", id); 
                return NotFound("User not found.");
            }

            _logger.LogInformation("Successfully fetched user data for ID {Id}", id);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id}")]
        public async Task<IActionResult> UpdateUserAsAdmin(string id, [FromBody] UpdateUserAdminDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating user with ID {Id}", id); 
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Admin updating user with ID {Id}", id); 
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for ID {Id}", id); 
                return NotFound("User not found.");
            }

            var result = await _userService.UpdateUserAdminAsync(user, updateUserDTO);

            if (result.Succeeded)
            {
                _logger.LogInformation("Successfully updated user with ID {Id}", id); 
                return Ok($"User {user.UserName} updated successfully.");
            }
            _logger.LogWarning("Admin failed to update user with ID {Id}. Errors: {Errors}", id, result.Errors);

            result.ToModelResult(ModelState);
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut("my-account")]
        public async Task<IActionResult> UpdateOwnAccount([FromBody] UpdateUserDTO updateOwnAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating own account."); 
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogWarning("Unauthorized attempt to update account. User ID not found."); 
                return Unauthorized("User ID not found.");
            }

            _logger.LogInformation("User {UserId} is updating their own account.", userId); 
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found for account update.", userId); 
                return NotFound("User not found.");
            }

            var result = await _userService.UpdateUserAsync(user, updateOwnAccountDTO);

            if (result.Succeeded)
            {
                _logger.LogInformation("Successfully updated own account for user {UserId}.", userId); 
                return Ok($"Your account {user.UserName} has been updated successfully.");
            }
            _logger.LogWarning("Failed to update own account for user {UserId}. Errors: {Errors}", userId, result.Errors);
            result.ToModelResult(ModelState);
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Admin attempting to delete user with ID {Id}", id); 

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {Id} not found for deletion.", id); 
                return NotFound($"User with Id = {id} not found");
            }

            await _userService.DeleteUserAsync(user);
            _logger.LogInformation("Successfully deleted user with ID {Id}.", id); 
            return Ok("User deleted successfully.");
        }
    }
}
