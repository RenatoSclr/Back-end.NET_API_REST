using Dot.Net.WebApi.Domain;
using System.Threading.Tasks;

namespace P7CreateRestApi.Services.IService
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);
    }
}