using System;
using System.Collections.Generic;

namespace codetheory.DAL.Models;

public partial class Question
{
    public int Id { get; set; }

    public int? LessonId { get; set; }

    public string QuestionText { get; set; } = null!;

    public int? QuestionOrder { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Lesson? Lesson { get; set; }
}
