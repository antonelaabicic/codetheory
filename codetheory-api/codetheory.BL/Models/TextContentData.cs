namespace codetheory.BL.Models
{
    public class TextContentData
    {
        public string? Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<Bullet>? Bullets { get; set; }
    }
}
