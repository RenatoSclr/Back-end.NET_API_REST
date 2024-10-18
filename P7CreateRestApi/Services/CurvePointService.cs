using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain.IRepositories;
using Dot.Net.WebApi.Services.IService;
using P7CreateRestApi.Domain.DTO.CurvePointDtos;

namespace Dot.Net.WebApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task<List<ReadCurvePointAdminDTO>> GetAllCurvePointDTOsAsAdminAsync()
        {
            var curvePointList = await _curvePointRepository.GetAllAsync();
            return MapToCurvePointAdminDTOList(curvePointList.ToList());
        }

        public async Task<List<ReadCurvePointDTO>> GetAllCurvePointDTOsAsUserAsync()
        {
            var curvePointList = await _curvePointRepository.GetAllAsync();
            return MapToCurvePointUserDTOList(curvePointList.ToList());
        }

        public async Task CreateCurvePointAsAdminAsync(CreateCurvePointAdminDTO curvePointDTO)
        {
            var curvePoint = MapCreatedCurvePointDTOToCurvePoint(curvePointDTO);
            await _curvePointRepository.AddAsync(curvePoint);
            await _curvePointRepository.SaveAsync();
        }

        public async Task UpdateCurvePointAsAdminAsync(UpdateCurvePointAdminDTO curvePointDTO, CurvePoint existingCurvePoint)
        {
            var curvePoint = MapUpdatedCurvePointDTOToCurvePoint(curvePointDTO, existingCurvePoint);
            await _curvePointRepository.UpdateAsync(curvePoint);
            await _curvePointRepository.SaveAsync();
        }

        public async Task<CurvePoint> GetCurvePointByIdAsync(int id)
        {
            return await _curvePointRepository.GetByIdAsync(id);
        }


        public async Task<ReadCurvePointAdminDTO> GetCurvePointDTOByIdAsync(int id)
        {
            var curvePoint = await _curvePointRepository.GetByIdAsync(id);
            return MapToCurvePointAdminDTO(curvePoint);
        }

        public async Task DeleteCurvePointAsync(int id)
        {
            await _curvePointRepository.DeleteAsync(id);
            await _curvePointRepository.SaveAsync();
        }


        private CurvePoint MapCreatedCurvePointDTOToCurvePoint(CreateCurvePointAdminDTO curvePointDTO)
        {
            var curvePoint = new CurvePoint();
            curvePoint.CurvePointValue = curvePointDTO.CurvePointDTOValue;
            curvePoint.CurveId = curvePointDTO.CurveDTOId;
            curvePoint.AsOfDate = curvePointDTO.AsOfDate;
            curvePoint.CreationDate = DateTime.Now;
            curvePoint.Term = curvePointDTO.Term;
            return curvePoint;
        }

        private CurvePoint MapUpdatedCurvePointDTOToCurvePoint(UpdateCurvePointAdminDTO curvePointDTO, CurvePoint existingCurvePoint)
        {
            var curvePoint = existingCurvePoint;
            curvePoint.CurvePointValue = curvePointDTO.CurvePointDTOValue;
            curvePoint.CurveId = curvePointDTO.CurveDTOId;
            curvePoint.AsOfDate = curvePointDTO.AsOfDate;
            curvePoint.Term = curvePointDTO.Term;
            return curvePoint;
        }

        private ReadCurvePointAdminDTO MapToCurvePointAdminDTO(CurvePoint curvePoint)
        {
            return new ReadCurvePointAdminDTO
            {
                Id = curvePoint.Id,
                CurvePointDTOValue = curvePoint.CurvePointValue,
                CurveDTOId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                CreationDate = curvePoint.CreationDate,
                Term = curvePoint.Term
            };
        }

        private List<ReadCurvePointAdminDTO> MapToCurvePointAdminDTOList(List<CurvePoint> curvePointList)
        {
            return curvePointList.Select(curvePoint => new ReadCurvePointAdminDTO
            {
                Id = curvePoint.Id,
                CurvePointDTOValue = curvePoint.CurvePointValue,
                CurveDTOId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                CreationDate = curvePoint.CreationDate,
                Term = curvePoint.Term
            }).ToList();
        }

        private List<ReadCurvePointDTO> MapToCurvePointUserDTOList(List<CurvePoint> curvePointList)
        {
            return curvePointList.Select(curvePoint => new ReadCurvePointDTO
            {
                CurvePointDTOValue = curvePoint.CurvePointValue,
                AsOfDate = curvePoint.AsOfDate,
                Term = curvePoint.Term
            }).ToList();
        }
    }
}
