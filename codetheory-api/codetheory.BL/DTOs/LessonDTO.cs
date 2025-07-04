namespace codetheory.BL.DTOs
{
    public class LessonDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public int? LessonOrder { get; set; }

        public List<LessonContentDto>? Contents { get; set; }
    }
}