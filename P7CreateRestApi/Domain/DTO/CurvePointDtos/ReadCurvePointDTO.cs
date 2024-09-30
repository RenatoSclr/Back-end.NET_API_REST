namespace P7CreateRestApi.Domain.DTO.CurvePointDtos
{
    public class ReadCurvePointDTO
    {
        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? CurvePointDTOValue { get; set; }
    }
}
