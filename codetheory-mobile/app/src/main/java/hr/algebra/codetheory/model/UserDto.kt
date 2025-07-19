package hr.algebra.codetheory.model

data class UserDto (
    val id: Int,
    val username: String,
    val email: String,
    val firstName: String?,
    val lastName: String?,
    val imagePath: String?
)
