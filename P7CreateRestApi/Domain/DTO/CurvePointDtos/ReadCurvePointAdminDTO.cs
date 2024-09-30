namespace P7CreateRestApi.Domain.DTO.CurvePointDtos
{
    public class ReadCurvePointAdminDTO
    {
        public int Id { get; set; }
        public int? CurveDTOId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? CurvePointDTOValue { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
