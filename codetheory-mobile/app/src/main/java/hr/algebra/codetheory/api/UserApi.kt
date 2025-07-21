package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.UserAnswerDto
import hr.algebra.codetheory.model.UserDto
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path

interface UserApi {
    @GET("api/User/by-username/{username}")
    suspend fun getUserByUsername(@Path("username") username: String): UserDto

    @GET("api/UserAnswer/user/{userId}/lesson/{lessonId}")
    suspend fun getUserAnswers(@Path("userId") userId: Int, @Path("lessonId") lessonId: Int): List<UserAnswerDto>

    @POST("api/UserAnswer/user/{userId}/lesson/{lessonId}")
    suspend fun postUserAnswers(@Path("userId") userId: Int, @Path("lessonId") lessonId: Int, @Body answers: List<UserAnswerDto>)

    @PUT("api/UserAnswer/user/{userId}/lesson/{lessonId}")
    suspend fun putUserAnswers(@Path("userId") userId: Int, @Path("lessonId") lessonId: Int, @Body answers: List<UserAnswerDto>)
}
