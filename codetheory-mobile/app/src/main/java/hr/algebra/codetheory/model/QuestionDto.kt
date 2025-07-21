package hr.algebra.codetheory.model

data class QuestionDto(
    val id: Int,
    val questionText: String,
    val questionOrder: Int,
    val answers: List<AnswerDto>
)
