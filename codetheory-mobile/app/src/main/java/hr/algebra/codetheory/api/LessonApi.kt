package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.LessonDto
import retrofit2.http.GET

interface LessonApi {
    @GET("/api/Lesson")
    suspend fun getAllLessons(): List<LessonDto>
}
