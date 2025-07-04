﻿namespace codetheory.DAL.Models;

public partial class ContentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
}
