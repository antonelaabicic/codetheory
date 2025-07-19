package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.UserProgressDto
import retrofit2.http.GET
import retrofit2.http.Path

interface UserProgressApi {
    @GET("api/UserAnswer/user/{userId}/progress")
    suspend fun getProgressForUser(@Path("userId") userId: Int): List<UserProgressDto>
}
