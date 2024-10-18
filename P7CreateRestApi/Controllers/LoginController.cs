using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain.DTO;
using P7CreateRestApi.Services.IService;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginController> _logger;  

        public LoginController(UserManager<User> userManager, ITokenService tokenService, ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;  
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginDTO.Username);  

            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {Username} - User not found", loginDTO.Username);  
                return Unauthorized("Invalid username or password.");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                _logger.LogWarning("Login failed for user: {Username} - Incorrect password", loginDTO.Username);  
                return Unauthorized("Invalid username or password.");
            }

            _logger.LogInformation("Login successful for user: {Username}", loginDTO.Username); 

            var token = await _tokenService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
