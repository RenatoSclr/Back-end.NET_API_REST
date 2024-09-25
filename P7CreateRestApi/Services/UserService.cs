using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Domain.DTO;
using P7CreateRestApi.Services.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P7CreateRestApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDataAsAdminDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDataDTOs = await Task.WhenAll(users.Select(u => MapUserToUserDataAsAdminDTO(u)));
            return userDataDTOs.ToList();
        }

        public async Task<UserDataAsAdminDTO> GetUserDataAsAdminDTOByIdAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            return  user != null ? await MapUserToUserDataAsAdminDTO(user) : null;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id); 
        }

        public async Task<IdentityResult> CreateUserWithDefaultRoleAsync(UserDTO userDTO, string password)
        {
            var user = MapUserDTOToUser(userDTO);

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User"); 
            }
            return result;
        }

        public async Task<IdentityResult> CreateUserAsAdminAsync(UserDTO userDTO, string password, string role)
        {
            var user = MapUserDTOToUser(userDTO);

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);
                
            }
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user, UserDataAsAdminDTO updateUserAsAdminDTO)
        {
            user.UserName = updateUserAsAdminDTO.UserName ?? user.UserName;
            user.Email = updateUserAsAdminDTO.Email ?? user.Email;
            user.FullName = updateUserAsAdminDTO.FullName ?? user.FullName;

            if (updateUserAsAdminDTO.Roles != null && updateUserAsAdminDTO.Roles.Any())
            {
               
                foreach (var role in updateUserAsAdminDTO.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var currentRoles = await _userManager.GetRolesAsync(user);

                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return removeRolesResult; 
                }

                var addRolesResult = await _userManager.AddToRolesAsync(user, updateUserAsAdminDTO.Roles);
                if (!addRolesResult.Succeeded)
                {
                    return addRolesResult; 
                }
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateOwnAccountAsync(User user, UpdateUserDTO updateOwnAccountDTO)
        {
            user.UserName = updateOwnAccountDTO.UserName ?? user.UserName;
            user.Email = updateOwnAccountDTO.Email ?? user.Email;
            user.FullName = updateOwnAccountDTO.FullName ?? user.FullName;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        private User MapUserDTOToUser(UserDTO userDTO)
        {
            return new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FullName = userDTO.FullName
            };
        }

        private async Task<UserDataAsAdminDTO> MapUserToUserDataAsAdminDTO(User user)
        {
            return new UserDataAsAdminDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Roles = await _userManager.GetRolesAsync(user)  
            };
        }
    }
}
