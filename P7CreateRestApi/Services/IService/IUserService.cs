using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain.DTO.UserDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Services.IService
{
    public interface IUserService
    {
        Task<List<ReadUserAdminDTO>> GetAllUsersForAdminAsync();
        Task<ReadUserAdminDTO> GetUserAdminDTOByIdAsync(string id);
        Task<ReadUserDTO> GetUserDTOByIdAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsAdminAsync(CreateUserAdminDTO userDTO);
        Task<IdentityResult> CreateUserWithDefaultRoleAsync(CreateUserDTO userDTO);
        Task<IdentityResult> UpdateUserAdminAsync(User user, UpdateUserAdminDTO updateUserAdminDTO);
        Task<IdentityResult> UpdateUserAsync(User user, UpdateUserDTO updateUserDTO);
        Task<IdentityResult> DeleteUserAsync(User user);
    }
}
