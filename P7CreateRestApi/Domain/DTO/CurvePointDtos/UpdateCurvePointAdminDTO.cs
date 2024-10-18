using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Domain.DTO.CurvePointDtos
{
    public class UpdateCurvePointAdminDTO
    {
        [Required(ErrorMessage = "CurveDTOId is required.")]
        public int? CurveDTOId { get; set; }

        [Required(ErrorMessage = "AsOfDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? AsOfDate { get; set; }

        [Required(ErrorMessage = "Term is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Term must be a positive number.")]
        public double? Term { get; set; }

        [Required(ErrorMessage = "CurvePointDTOValue is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "CurvePointDTOValue must be a positive number.")]
        public double? CurvePointDTOValue { get; set; }
    }
}
