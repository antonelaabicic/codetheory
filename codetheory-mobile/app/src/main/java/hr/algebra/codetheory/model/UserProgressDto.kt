package hr.algebra.codetheory.model

data class UserProgressDto (
    val userId: Int,
    val lessonId: Int,
    val isCompleted: Boolean?,
    val score: Float?
)
