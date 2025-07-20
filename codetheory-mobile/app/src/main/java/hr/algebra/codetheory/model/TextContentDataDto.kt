package hr.algebra.codetheory.model

data class TextContentDataDto(
    val title: String? = null,
    val text: String,
    val bullets: List<BulletDto>? = null
)
