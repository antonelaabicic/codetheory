package hr.algebra.codetheory.ui.learn

import android.os.Bundle
import android.text.Html
import android.view.*
import android.webkit.WebView
import android.webkit.WebViewClient
import android.widget.*
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.google.gson.Gson
import hr.algebra.codetheory.MainActivity
import hr.algebra.codetheory.R
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.QuestionApi
import hr.algebra.codetheory.api.UserApi
import hr.algebra.codetheory.databinding.FragmentQuizBinding
import hr.algebra.codetheory.framework.TokenManager
import hr.algebra.codetheory.model.QuestionDto
import hr.algebra.codetheory.model.UserAnswerDto
import kotlinx.coroutines.launch

class QuizFragment : Fragment() {

    private var _binding: FragmentQuizBinding? = null
    private val binding get() = _binding!!

    private val gson = Gson()
    private val questionApi by lazy { ApiClient.getApi(QuestionApi::class.java, requireContext()) }
    private val userApi by lazy { ApiClient.getApi(UserApi::class.java, requireContext()) }

    private val selectedAnswers = mutableMapOf<Int, Int>()
    private var totalQuestions = 0
    private var userId: Int = -1
    private var quizId: Int = -1
    private var hasPreviouslyAnswered = false

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        _binding = FragmentQuizBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        quizId = requireArguments().getInt("quizId")

        binding.submitButton.isEnabled = false
        binding.submitButton.alpha = 0.5f

        fetchQuestionsAndAnswers(quizId)

        binding.submitButton.setOnClickListener {
            submitAnswers()
        }
    }

    private fun fetchQuestionsAndAnswers(lessonId: Int) {
        lifecycleScope.launch {
            try {
                val username = TokenManager.getUsername(requireContext()) ?: return@launch
                val user = userApi.getUserByUsername(username)
                userId = user.id

                val previousAnswers = userApi.getUserAnswers(userId, lessonId)
                val selectedAnswerIds = previousAnswers.map { it.answerId }.toSet()
                hasPreviouslyAnswered = selectedAnswerIds.isNotEmpty()

                val questions = questionApi.getQuizQuestions(lessonId).sortedBy { it.questionOrder }
                totalQuestions = questions.size

                questions.forEach { question ->
                    displayQuestion(question, selectedAnswerIds)
                }

                checkSubmitState()

            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }

    private fun displayQuestion(question: QuestionDto, selectedAnswerIds: Set<Int>) {
        val container = binding.quizContainer
        val questionData = gson.fromJson(question.questionText, Map::class.java)

        val layout = LinearLayout(requireContext()).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(0, 48, 0, 48)
        }

        val questionView = TextView(requireContext()).apply {
            text = questionData["prompt"]?.toString() ?: ""
            textSize = 18f
            setTypeface(null, android.graphics.Typeface.BOLD)
            setTextColor(ContextCompat.getColor(requireContext(), R.color.bootstrap_green))
            setPadding(0, 0, 0, 24)
        }
        layout.addView(questionView)

        questionData["image_path"]?.toString()?.let { imagePath ->
            val imageView = ImageView(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, 700)
                adjustViewBounds = true
            }
            com.squareup.picasso.Picasso.get().load(imagePath).into(imageView)
            layout.addView(imageView)
            layout.addView(makeSpacer())
        }

        questionData["code"]?.toString()?.let { code ->
            val webView = WebView(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT)
                settings.javaScriptEnabled = true
                webViewClient = WebViewClient()
            }
            webView.loadDataWithBaseURL(null, generateHtml(code), "text/html", "UTF-8", null)
            layout.addView(webView)
            layout.addView(makeSpacer())
        }

        val radioGroup = RadioGroup(requireContext()).apply {
            orientation = RadioGroup.VERTICAL
            setPadding(0, 0, 0, 24)
        }

        var lastCheckedId: Int? = null

        question.answers.forEach { answer ->
            val radioButton = RadioButton(requireContext()).apply {
                text = answer.answerText
                tag = answer.id
                textSize = 18f
                id = View.generateViewId()
                setPadding(16, 16, 16, 16)
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    bottomMargin = 12
                }

                if (answer.id in selectedAnswerIds) {
                    isChecked = true
                    lastCheckedId = id
                    selectedAnswers[question.id] = answer.id
                }

                setOnClickListener {
                    if (isChecked && lastCheckedId == id) {
                        radioGroup.clearCheck()
                        selectedAnswers.remove(question.id)
                        lastCheckedId = null
                    } else {
                        selectedAnswers[question.id] = answer.id
                        lastCheckedId = id
                    }
                    checkSubmitState()
                }
            }
            radioGroup.addView(radioButton)
        }

        layout.addView(radioGroup)
        container.addView(layout)
    }

    private fun checkSubmitState() {
        val enabled = if (hasPreviouslyAnswered) {
            selectedAnswers.isNotEmpty()
        } else {
            selectedAnswers.size == totalQuestions
        }

        binding.submitButton.isEnabled = enabled
        binding.submitButton.alpha = if (enabled) 1f else 0.5f
    }

    private fun submitAnswers() {
        lifecycleScope.launch {
            try {
                val answers = selectedAnswers.map { (_, answerId) ->
                    UserAnswerDto(userId = userId, answerId = answerId)
                }

                if (hasPreviouslyAnswered) {
                    userApi.putUserAnswers(userId, quizId, answers)
                } else {
                    userApi.postUserAnswers(userId, quizId, answers)
                }

                Toast.makeText(requireContext(), "Quiz submitted!", Toast.LENGTH_SHORT).show()
                findNavController().popBackStack(R.id.navigation_learn, false)

                (requireActivity() as MainActivity)
                    .findViewById<BottomNavigationView>(R.id.nav_view)
                    .selectedItemId = R.id.navigation_account

            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Submission failed", Toast.LENGTH_SHORT).show()
                e.printStackTrace()
            }
        }
    }

    private fun generateHtml(code: String): String {
        val escapedCode = Html.escapeHtml(code)
        return """
            <html>
            <head>
                <meta charset="utf-8"/>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/themes/prism-tomorrow.min.css"/>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/line-numbers/prism-line-numbers.min.css"/>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/prism.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/line-numbers/prism-line-numbers.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-python.min.js"></script>
            </head>
            <body style="margin:0;padding:16px;background-color:#2d2d2d;color:white;font-family:monospace;">
                <pre class="line-numbers"><code class="language-python">$escapedCode</code></pre>
            </body>
            </html>
        """.trimIndent()
    }

    private fun makeSpacer(height: Int = 24): Space {
        return Space(requireContext()).apply {
            layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, height)
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
