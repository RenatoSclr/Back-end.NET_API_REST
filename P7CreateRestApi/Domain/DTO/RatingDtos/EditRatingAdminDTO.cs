namespace P7CreateRestApi.Domain.DTO.RatingDtos
{
    public class EditRatingAdminDTO
    {
        public string MoodysRating { get; set; }
        public string SandPRating { get; set; }
        public string FitchRating { get; set; }
        public int? OrderNumber { get; set; }
    }
}
