using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO.UserDtos;
using P7CreateRestApi.Services.IService;
using System.Security.Claims;

namespace P7CreateRestApi.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserWithDefaultRoleAsync(createUserDTO);

            if (result.Succeeded)
            {
                return Ok($"User {createUserDTO.UserName} created with role 'User'");
            }
            return BadRequest(result.Errors);
            
        }

       

        [Authorize(Roles = "Admin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateUserAsAdmin([FromBody] CreateUserAdminDTO createUserDTO)
        {
            var result = await _userService.CreateUserAsAdminAsync(createUserDTO);

            if (result.Succeeded)
            {
                return Ok($"User {createUserDTO.UserName} created with role {createUserDTO.Roles}");
            }
            return BadRequest(result.Errors);
               
        }

        [Authorize]
        [HttpGet("my-account")]
        public async Task<IActionResult> GetUserSelfData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found.");
            }

            var user = await _userService.GetUserDTOByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersForAdminAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsAdminById(string id)
        {
            var user = await _userService.GetUserAdminDTOByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUserAsAdmin(string id, [FromBody] UpdateUserAdminDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userService.UpdateUserAdminAsync(user, updateUserDTO);
           
            if (result.Succeeded)
            {
                return Ok($"User {user.UserName} updated successfully."); ;
            }
            return BadRequest(result.Errors);
        }


        [Authorize]
        [HttpPut("update-my-account")]
        public async Task<IActionResult> UpdateOwnAccount([FromBody] UpdateUserDTO updateOwnAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userService.UpdateUserAsync(user, updateOwnAccountDTO);
           
            if (result.Succeeded)
            {
                return Ok($"Your account {user.UserName} has been updated successfully.");
            }
            return BadRequest(result.Errors);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            await _userService.DeleteUserAsync(user);
            return Ok("User deleted successfully.");
        }
    }
}
