package hr.algebra.codetheory.model

data class LessonContentDto (
    val id: Int,
    val lessonId: Int?,
    val contentTypeId: Int?,
    val contentData: String,
    val contentOrder: Int?
)
