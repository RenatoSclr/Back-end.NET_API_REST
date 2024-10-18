using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Services.IService
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);
    }
}