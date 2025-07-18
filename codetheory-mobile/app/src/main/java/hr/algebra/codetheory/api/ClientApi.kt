@file:Suppress("UNCHECKED_CAST")

package hr.algebra.codetheory.api

import android.content.Context
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object ApiClient {

    private const val BASE_URL = "https://codetheory-api.onrender.com/"

    private val retrofitMap = mutableMapOf<Class<*>, Any>()

    private fun createClient(withAuth: Boolean, context: Context?): OkHttpClient {
        val builder = OkHttpClient.Builder()
        if (withAuth && context != null) {
            builder.addInterceptor(AuthInterceptor(context))
        }
        return builder.build()
    }

    fun <T : Any> getApi(service: Class<T>, context: Context? = null): T {
        val withAuth = context != null

        return retrofitMap.getOrPut(service) {
            Retrofit.Builder()
                .baseUrl(BASE_URL)
                .client(createClient(withAuth, context))
                .addConverterFactory(GsonConverterFactory.create())
                .build()
                .create(service)
        } as T
    }
}
