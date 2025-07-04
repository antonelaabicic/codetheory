using System;
using System.Collections.Generic;

namespace codetheory.DAL.Models;

public partial class LessonContent
{
    public int Id { get; set; }

    public int? LessonId { get; set; }

    public int? ContentTypeId { get; set; }

    public string ContentData { get; set; } = null!;

    public int? ContentOrder { get; set; }

    public virtual ContentType? ContentType { get; set; }

    public virtual Lesson? Lesson { get; set; }
}
