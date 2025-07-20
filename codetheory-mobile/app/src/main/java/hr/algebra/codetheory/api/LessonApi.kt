package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.LessonDto
import retrofit2.http.GET
import retrofit2.http.Path

interface LessonApi {
    @GET("/api/Lesson")
    suspend fun getAllLessons(): List<LessonDto>

    @GET("/api/Lesson/{lessonId}")
    suspend fun getLesson(@Path("lessonId") id: Int): LessonDto
}
