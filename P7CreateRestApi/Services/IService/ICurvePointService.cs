using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;

namespace Dot.Net.WebApi.Services.IService
{
    public interface ICurvePointService
    {
        Task<List<ReadCurvePointAdminDTO>> GetAllCurvePointDTOsAsAdminAsync();
        Task<List<ReadCurvePointDTO>> GetAllCurvePointDTOsAsUserAsync();

        Task CreateCurvePointAsAdminAsync(CreateCurvePointAdminDTO bidDTO);
        

        Task<CurvePoint> GetCurvePointByIdAsync(int id);

        Task<ReadCurvePointAdminDTO> GetCurvePointDTOByIdAsync(int id);

        Task UpdateCurvePointAsAdminAsync(UpdateCurvePointAdminDTO bidDTO, CurvePoint bid);

        Task DeleteCurvePointAsync(int id);
    }
}
