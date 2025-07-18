package hr.algebra.codetheory.framework

import android.content.Context
import android.util.Base64
import org.json.JSONObject
import androidx.core.content.edit

private const val TOKEN_KEY = "auth_token"
private const val PREF_NAME = "code_theory_prefs"

object TokenManager {

    fun saveToken(context: Context, token: String) {
        context.pref().edit { putString(TOKEN_KEY, token) }
    }

    fun getToken(context: Context): String? {
        return context.pref().getString(TOKEN_KEY, null)
    }

    fun clearToken(context: Context) {
        context.pref().edit { remove(TOKEN_KEY) }
    }

    fun getRole(context: Context): String? = getClaim(context, "role")

    fun getUsername(context: Context): String? = getClaim(context, "unique_name")

    private fun getClaim(context: Context, claim: String): String? {
        val token = getToken(context) ?: return null
        val parts = token.split(".")
        if (parts.size != 3) return null

        return try {
            val payload = String(Base64.decode(parts[1], Base64.URL_SAFE or Base64.NO_WRAP or Base64.NO_PADDING))
            JSONObject(payload).optString(claim)
        } catch (e: Exception) {
            null
        }
    }
}
