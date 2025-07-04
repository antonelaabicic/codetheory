namespace codetheory.BL.DTOs
{
    public class LessonContentDto
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public int? ContentTypeId { get; set; }
        public string ContentData { get; set; } = string.Empty;
        public int? ContentOrder { get; set; }
    }
}