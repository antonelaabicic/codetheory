package hr.algebra.codetheory.model

data class LessonDto(
    val id: Int,
    val title: String,
    val summary: String?,
    val lessonOrder: Int?,
)