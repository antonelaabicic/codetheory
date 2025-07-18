package hr.algebra.codetheory.api

import hr.algebra.codetheory.model.LoginDto
import hr.algebra.codetheory.model.TokenResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthApi {
    @POST("api/Auth/login")
    suspend fun login(@Body dto: LoginDto): Response<TokenResponse>
}
