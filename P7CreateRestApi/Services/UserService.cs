using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Domain.DTO.UserDtos;
using P7CreateRestApi.Services.IService;

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

        public async Task<List<ReadUserAdminDTO>> GetAllUsersForAdminAsync()
        {
            var users = GetAllUsers();

            var userDataDTOs = new List<ReadUserAdminDTO>();

            foreach (var user in users)
            {
                var userData = await MapUserToReadUserAdminDTO(user);
                userDataDTOs.Add(userData);
            }

            return userDataDTOs;
        }

        private List<User> GetAllUsers()
        {
            var users =_userManager.Users.AsNoTracking().ToList();
            return users;
        }

        public async Task<ReadUserAdminDTO> GetUserAdminDTOByIdAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            return  user != null ? await MapUserToReadUserAdminDTO(user) : null;
        }

        public async Task<ReadUserDTO> GetUserDTOByIdAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            return user != null ? await MapUserToReadUserDTO(user) : null;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id); 
        }

        public async Task<IdentityResult> CreateUserWithDefaultRoleAsync(CreateUserDTO createUserDTO)
        {
            var user = MapCreateUserDTOToUser(createUserDTO);

            var result = await _userManager.CreateAsync(user, createUserDTO.Password);

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

        public async Task<IdentityResult> CreateUserAsAdminAsync(CreateUserAdminDTO createdUserAdminDTO)
        {
            var user = MapCreateUserAdminDTOToUser(createdUserAdminDTO);

            var result = await _userManager.CreateAsync(user, createdUserAdminDTO.Password);

            if (result.Succeeded)
            {
                if (createdUserAdminDTO.Roles != null && createdUserAdminDTO.Roles.Any())
                {
                    foreach (var role in createdUserAdminDTO.Roles)
                    {
                        if (!await _roleManager.RoleExistsAsync(role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }

                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                else 
                {
                    await _userManager.AddToRoleAsync(user, "User"); 
                }    
            }
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAdminAsync(User user, UpdateUserAdminDTO updateUserAdminDTO)
        {
            user.UserName = updateUserAdminDTO.UserName ?? user.UserName;
            user.Email = updateUserAdminDTO.Email ?? user.Email;
            user.FullName = updateUserAdminDTO.FullName ?? user.FullName;

            if (updateUserAdminDTO.Roles != null && updateUserAdminDTO.Roles.Any())
            {
                foreach (var role in updateUserAdminDTO.Roles)
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

                var addRolesResult = await _userManager.AddToRolesAsync(user, updateUserAdminDTO.Roles);
                if (!addRolesResult.Succeeded)
                {
                    return addRolesResult; 
                }
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user, UpdateUserDTO updateUsertDTO)
        {
            user.UserName = updateUsertDTO.UserName ?? user.UserName;
            user.Email = updateUsertDTO.Email ?? user.Email;
            user.FullName = updateUsertDTO.FullName ?? user.FullName;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        private User MapCreateUserDTOToUser(CreateUserDTO createUserDTO)
        {
            return new User
            {
                UserName = createUserDTO.UserName,
                Email = createUserDTO.Email,
                FullName = createUserDTO.FullName,
            };
        }

        private User MapCreateUserAdminDTOToUser(CreateUserAdminDTO createUserAdminDTO)
        {
            return new User
            {
                UserName = createUserAdminDTO.UserName,
                Email = createUserAdminDTO.Email,
                FullName = createUserAdminDTO.FullName,
            };
        }

        private async Task<ReadUserAdminDTO> MapUserToReadUserAdminDTO(User user)
        {
            return new ReadUserAdminDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Roles = await _userManager.GetRolesAsync(user)  
            };
        }

        private async Task<ReadUserDTO> MapUserToReadUserDTO(User user)
        {
            return new ReadUserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }
    }
}
