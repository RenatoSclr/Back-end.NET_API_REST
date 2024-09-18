using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services.IService
{
    public interface ICurvePointService
    {
        Task<List<CurvePointDTO>> GetAllCurvePointDTOsAsync();

        Task CreateCurvePointAsync(CurvePointDTO bidDTO);

        Task<CurvePoint> GetCurvePointByIdAsync(int id);

        Task<CurvePointDTO> GetCurvePointDTOByIdAsync(int id);

        Task UpdateCurvePointAsync(CurvePointDTO bidDTO, CurvePoint bid);

        Task DeleteCurvePointAsync(int id);
    }
}
