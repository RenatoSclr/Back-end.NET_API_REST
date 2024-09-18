using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Repositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO;

namespace Dot.Net.WebApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task<List<CurvePointDTO>> GetAllCurvePointDTOsAsync()
        {
            var curvePointList = await _curvePointRepository.GetAllAsync();
            return MapToCurvePointDTOList(curvePointList.ToList());
        }

        public async Task CreateCurvePointAsync(CurvePointDTO curvePointDTO)
        {
            var curvePoint = MapToCurvePoint(curvePointDTO);
            await _curvePointRepository.AddAsync(curvePoint);
            await _curvePointRepository.SaveAsync();
        }

        public async Task UpdateCurvePointAsync(CurvePointDTO curvePointDTO, CurvePoint existingCurvePoint)
        {
            var curvePoint = MapToCurvePoint(curvePointDTO, existingCurvePoint);
            await _curvePointRepository.UpdateAsync(curvePoint);
            await _curvePointRepository.SaveAsync();
        }

        public async Task<CurvePoint> GetCurvePointByIdAsync(int id)
        {
            return await _curvePointRepository.GetByIdAsync(id);
        }


        public async Task<CurvePointDTO> GetCurvePointDTOByIdAsync(int id)
        {
            var curvePoint = await _curvePointRepository.GetByIdAsync(id);
            return MapToCurvePointDTO(curvePoint);
        }

        public async Task DeleteCurvePointAsync(int id)
        {
            await _curvePointRepository.DeleteAsync(id);
            await _curvePointRepository.SaveAsync();
        }


        private CurvePoint MapToCurvePoint(CurvePointDTO curvePointDTO, CurvePoint existingCurvePoint = null)
        {
            var curvePoint = existingCurvePoint ?? new CurvePoint();
            
            curvePoint.CurvePointValue = curvePointDTO.CurvePointDTOValue;
            curvePoint.CurveId = curvePointDTO.CurveDTOId;
            curvePoint.AsOfDate = curvePointDTO.AsOfDate;
            curvePoint.CreationDate = curvePointDTO.CreationDate;
            curvePoint.Term = curvePointDTO.Term;
            return curvePoint;
        }

        private CurvePointDTO MapToCurvePointDTO(CurvePoint curvePoint)
        {
            return new CurvePointDTO
            {
                CurvePointDTOValue = curvePoint.CurvePointValue,
                CurveDTOId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                CreationDate = curvePoint.CreationDate,
                Term = curvePoint.Term
            };
        }

        private List<CurvePointDTO> MapToCurvePointDTOList(List<CurvePoint> curvePointList)
        {
            return curvePointList.Select(curvePoint => new CurvePointDTO
            {
                CurvePointDTOValue = curvePoint.CurvePointValue,
                CurveDTOId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                CreationDate = curvePoint.CreationDate,
                Term = curvePoint.Term
            }).ToList();
        }
    }
}
