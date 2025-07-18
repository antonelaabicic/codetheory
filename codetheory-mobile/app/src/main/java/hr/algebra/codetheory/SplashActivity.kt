package hr.algebra.codetheory

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.airbnb.lottie.LottieAnimationView
import hr.algebra.codetheory.framework.isOnline
import hr.algebra.codetheory.framework.callDelayed
import hr.algebra.codetheory.framework.startActivity

private const val DELAY = 5000L

class SplashActivity : AppCompatActivity() {

    private lateinit var codingAnimation: LottieAnimationView
    private lateinit var loadingAnimation: LottieAnimationView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash)

        codingAnimation = findViewById(R.id.codingAnimation)
        loadingAnimation = findViewById(R.id.loadingAnimation)

        if (isOnline()) {
            codingAnimation.setAnimationFromUrl("https://mvmmzqumvbjmmtcvrzxp.supabase.co/storage/v1/object/public/assets/lottie/coding.json")
            loadingAnimation.setAnimationFromUrl("https://mvmmzqumvbjmmtcvrzxp.supabase.co/storage/v1/object/public/assets/lottie/loading.json")

            codingAnimation.playAnimation()
            loadingAnimation.playAnimation()

            callDelayed(DELAY) {
                startActivity<LoginActivity>()
                finish()
            }

        } else {
            codingAnimation.cancelAnimation()
            loadingAnimation.setAnimationFromUrl("https://mvmmzqumvbjmmtcvrzxp.supabase.co/storage/v1/object/public/assets/lottie/no_internet.json")
            loadingAnimation.playAnimation()
        }
    }
}
