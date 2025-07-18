package hr.algebra.codetheory

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.*
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.AuthApi
import hr.algebra.codetheory.framework.TokenManager
import hr.algebra.codetheory.model.LoginDto
import hr.algebra.codetheory.model.TokenResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class LoginActivity : AppCompatActivity() {

    private lateinit var usernameInput: EditText
    private lateinit var passwordInput: EditText
    private lateinit var loginButton: Button
    private lateinit var errorText: TextView
    private lateinit var loading: ProgressBar

    private val authApi: AuthApi by lazy {
        ApiClient.getApi(AuthApi::class.java)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        usernameInput = findViewById(R.id.usernameInput)
        passwordInput = findViewById(R.id.passwordInput)
        loginButton = findViewById(R.id.loginButton)
        errorText = findViewById(R.id.errorText)

        loading = ProgressBar(this).apply {
            visibility = View.GONE
        }

        loginButton.setOnClickListener {
            errorText.visibility = View.GONE
            val username = usernameInput.text.toString().trim()
            val password = passwordInput.text.toString()

            if (username.isEmpty() || password.isEmpty()) {
                errorText.text = getString(R.string.wrong_username_or_password)
                errorText.visibility = View.VISIBLE
                return@setOnClickListener
            }

            performLogin(username, password)
        }
    }

    private fun performLogin(username: String, password: String) {
        loginButton.isEnabled = false

        lifecycleScope.launch {
            val response: Result<retrofit2.Response<TokenResponse>> = withContext(Dispatchers.IO) {
                runCatching {
                    authApi.login(LoginDto(username, password))
                }
            }

            loginButton.isEnabled = true

            response.onSuccess { resp ->
                if (resp.isSuccessful) {
                    val token = resp.body()
                    if (token != null) {
                        TokenManager.saveToken(this@LoginActivity, token.token)
                        goToMain()
                    } else {
                        showError()
                    }
                } else {
                    showError()
                }
            }.onFailure {
                showError()
            }
        }
    }

    private fun goToMain() {
        startActivity(Intent(this, MainActivity::class.java))
        finish()
    }

    private fun showError() {
        errorText.visibility = View.VISIBLE
    }
}
