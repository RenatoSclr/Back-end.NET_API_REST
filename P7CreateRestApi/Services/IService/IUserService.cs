using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Services.IService
{
    public interface IUserService
    {
        Task<List<UserDataAsAdminDTO>> GetAllUsersAsync();
        Task<UserDataAsAdminDTO> GetUserDataAsAdminDTOByIdAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsAdminAsync(UserDTO userDTO, string password, string role);
        Task<IdentityResult> CreateUserWithDefaultRoleAsync(UserDTO userDTO, string password);
        Task<IdentityResult> UpdateUserAsync(User user, UserDataAsAdminDTO updateUserAsAdminDTO);
        Task<IdentityResult> UpdateOwnAccountAsync(User user, UpdateUserDTO updateOwnAccountDTO);
        Task<IdentityResult> DeleteUserAsync(User user);
    }
}
