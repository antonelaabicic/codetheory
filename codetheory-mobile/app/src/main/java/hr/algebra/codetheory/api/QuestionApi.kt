package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.QuestionDto
import retrofit2.http.GET
import retrofit2.http.Path

interface QuestionApi {
    @GET("api/Question/{lessonId}/quiz")
    suspend fun getQuizQuestions(@Path("lessonId") lessonId: Int): List<QuestionDto>
}