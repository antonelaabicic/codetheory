package hr.algebra.codetheory.ui.learn

import android.os.Bundle
import android.text.Html
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.webkit.WebView
import android.webkit.WebViewClient
import android.widget.*
import androidx.core.content.ContextCompat
import androidx.core.view.setPadding
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.google.gson.Gson
import hr.algebra.codetheory.MainActivity
import hr.algebra.codetheory.R
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.QuestionApi
import hr.algebra.codetheory.databinding.FragmentQuizBinding
import hr.algebra.codetheory.model.AnswerDto
import hr.algebra.codetheory.model.QuestionDto
import kotlinx.coroutines.launch

class QuizFragment : Fragment() {

    private var _binding: FragmentQuizBinding? = null
    private val binding get() = _binding!!

    private val gson = Gson()
    private val questionApi by lazy {
        ApiClient.getApi(QuestionApi::class.java, requireContext())
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        _binding = FragmentQuizBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val quizId = requireArguments().getInt("quizId")
        fetchQuestions(quizId)

        binding.submitButton.setOnClickListener {
            Toast.makeText(requireContext(), "Quiz submitted!", Toast.LENGTH_SHORT).show()

            val navController = findNavController()
            navController.popBackStack(R.id.navigation_learn, false)

            (requireActivity() as MainActivity)
                .findViewById<BottomNavigationView>(R.id.nav_view)
                .selectedItemId = R.id.navigation_account
        }
    }

    private fun fetchQuestions(lessonId: Int) {
        lifecycleScope.launch {
            try {
                val questions = questionApi.getQuizQuestions(lessonId)
                questions.sortedBy { it.questionOrder }.forEach { displayQuestion(it) }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }

    private fun displayQuestion(question: QuestionDto) {
        val container = binding.quizContainer

        val questionData = gson.fromJson(question.questionText, Map::class.java)

        val questionLayout = LinearLayout(requireContext()).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(0, 48, 0, 48)
        }

        val questionView = TextView(requireContext()).apply {
            text = questionData["prompt"]?.toString() ?: ""
            textSize = 18f
            setTextColor(ContextCompat.getColor(requireContext(), R.color.bootstrap_green))
            setTypeface(null, android.graphics.Typeface.BOLD)
            setPadding(0, 0, 0, 24)
        }

        questionLayout.addView(questionView)

        if (questionData.containsKey("image_path")) {
            val imageView = ImageView(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    600
                )
                adjustViewBounds = true
            }
            val imageUrl = questionData["image_path"].toString()
            com.squareup.picasso.Picasso.get().load(imageUrl).into(imageView)
            questionLayout.addView(imageView)
            questionLayout.addView(Space(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, 24)
            })
        }

        if (questionData.containsKey("code")) {
            val webView = WebView(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                )
                settings.javaScriptEnabled = true
                webViewClient = WebViewClient()
            }

            val code = questionData["code"].toString()
            val html = generateHtml(code)
            webView.loadDataWithBaseURL(null, html, "text/html", "UTF-8", null)
            questionLayout.addView(webView)
            questionLayout.addView(Space(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, 24)
            })
        }

        val radioGroup = RadioGroup(requireContext()).apply {
            orientation = RadioGroup.VERTICAL
        }

        question.answers.forEach { answer ->
            val radioButton = RadioButton(requireContext()).apply {
                text = answer.answerText
                textSize = 17f
                setPadding(8)
            }
            radioGroup.addView(radioButton)
            val spacer = Space(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, 16)
            }
            questionLayout.addView(spacer)
        }

        questionLayout.addView(radioGroup)
        container.addView(questionLayout)
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
                <script>Prism.highlightAll();</script>
            </head>
            <body style="margin:0;padding:16px;background-color:#2d2d2d;color:white;font-family:monospace;">
                <pre class="line-numbers"><code class="language-clike">$escapedCode</code></pre>
            </body>
            </html>
        """.trimIndent()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
