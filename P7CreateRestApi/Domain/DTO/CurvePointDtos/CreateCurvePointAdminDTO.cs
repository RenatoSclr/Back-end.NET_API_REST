namespace P7CreateRestApi.Domain.DTO.CurvePointDtos
{
    public class CreateCurvePointAdminDTO
    {
        public int? CurveDTOId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? CurvePointDTOValue { get; set; }
    }
}
