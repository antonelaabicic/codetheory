package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.LessonDto
import retrofit2.http.GET
import retrofit2.http.Path

interface LessonApi {

    @GET("api/lesson")
    suspend fun getAllLessons(): List<LessonDto>

    @GET("api/lesson/{id}")
    suspend fun getLessonById(@Path("id") id: Int): LessonDto
}