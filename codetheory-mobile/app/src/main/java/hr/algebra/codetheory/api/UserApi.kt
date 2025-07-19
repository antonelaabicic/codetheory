package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.UserDto
import retrofit2.http.GET
import retrofit2.http.Path

interface UserApi {
    @GET("api/User/by-username/{username}")
    suspend fun getUserByUsername(@Path("username") username: String): UserDto
}
