namespace codetheory.Web.Models
{
    public class LessonDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int LessonOrder { get; set; }
        public List<LessonContentDTO> Contents { get; set; } = new();
    }
}
