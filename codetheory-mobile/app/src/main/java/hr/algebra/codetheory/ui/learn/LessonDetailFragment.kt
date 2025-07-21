package hr.algebra.codetheory.ui.learn

import android.annotation.SuppressLint
import android.os.Bundle
import android.text.Html.fromHtml
import android.util.Log
import android.view.*
import android.webkit.WebView
import android.webkit.WebViewClient
import android.widget.*
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.google.gson.Gson
import com.google.gson.JsonSyntaxException
import com.squareup.picasso.Picasso
import hr.algebra.codetheory.R
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.LessonApi
import hr.algebra.codetheory.databinding.FragmentLessonDetailBinding
import hr.algebra.codetheory.model.*
import hr.algebra.codetheory.util.VideoPlayerView
import kotlinx.coroutines.launch
import androidx.core.view.isVisible

@Suppress("DEPRECATION")
class LessonDetailFragment : Fragment() {

    private var _binding: FragmentLessonDetailBinding? = null
    private val binding get() = _binding!!
    private val gson = Gson()

    private val lessonApi by lazy {
        ApiClient.getApi(LessonApi::class.java, requireContext())
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLessonDetailBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val lessonId = requireArguments().getInt("lessonId")
        fetchLessonDetail(lessonId)

        binding.quizButton.setOnClickListener {
            val bundle = Bundle().apply { putInt("quizId", lessonId) }
            findNavController().navigate(R.id.action_lessonDetailFragment_to_quizFragment, bundle)
        }
    }

    private fun fetchLessonDetail(id: Int) {
        lifecycleScope.launch {
            try {
                val lesson = lessonApi.getLesson(id)
                (requireActivity() as AppCompatActivity).supportActionBar?.title = lesson.title

                lesson.summary?.let {
                    binding.lessonContentContainer.addView(makeTextView(it, false))
                    addSpacer(binding.lessonContentContainer)
                }

                lesson.contents.forEach { content ->
                    when (content.contentTypeId) {
                        1 -> parseAndDisplayText(content.contentData)
                        2 -> parseAndDisplayImage(content.contentData)
                        3 -> parseAndDisplayVideo(content.contentData)
                        4 -> parseAndDisplayCode(content.contentData)
                        else -> Log.w("CONTENT_DEBUG", "Unknown content type: ${content.contentTypeId}")
                    }
                }

            } catch (e: Exception) {
                Log.e("LESSON_DETAIL_ERROR", "Failed to fetch lesson: ${e.message}")
            }
        }
    }

    private fun parseAndDisplayText(json: String) {
        val container = binding.lessonContentContainer
        var contentHandled = false

        runCatching { gson.fromJson(json, JokeContentDataDto::class.java) }
            .getOrNull()?.takeIf {
                !it.question.isNullOrBlank() && !it.answer.isNullOrBlank()
            }?.let { joke ->
                container.addView(makeJokeView(joke.question, fromHtml(joke.answer).toString().replace("\\n", "\n")))
                contentHandled = true
            }

        if (contentHandled) {
            addSpacer(container)
            return
        }

        val text = gson.fromJson(json, TextContentDataDto::class.java)
        text.title?.let { container.addView(makeTextView(it, true)) }
        text.text?.replace("\\n", "\n")?.let {
            container.addView(makeTextView(it, false))
        }
        text.bullets?.forEach { bullet ->
            container.addView(makeTextView("• ${bullet.heading}", true))
            container.addView(makeTextView(bullet.text, false))
        }

        addSpacer(container)
    }

    private fun parseAndDisplayImage(json: String) {
        val image = gson.fromJson(json, ImageContentDataDto::class.java)
        val imageView = ImageView(requireContext()).apply {
            layoutParams = LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MATCH_PARENT,
                LinearLayout.LayoutParams.WRAP_CONTENT
            )
            adjustViewBounds = true
        }
        Picasso.get().load(image.image_path).into(imageView)
        binding.lessonContentContainer.addView(imageView)
        addSpacer(binding.lessonContentContainer)
    }

    private fun parseAndDisplayVideo(json: String) {
        val video = gson.fromJson(json, VideoContentDataDto::class.java)
        val videoView = VideoPlayerView(requireContext())
        videoView.setVideoUrl(video.url, autoplay = false)
        binding.lessonContentContainer.addView(videoView)
        addSpacer(binding.lessonContentContainer)
    }

    @SuppressLint("SetJavaScriptEnabled")
    private fun parseAndDisplayCode(json: String) {
        val rawSnippets = try {
            gson.fromJson(json, Array<CodeContentDataDto>::class.java).toList()
        } catch (e: JsonSyntaxException) {
            listOf(gson.fromJson(json, CodeContentDataDto::class.java))
        }

        if (rawSnippets.isEmpty()) return
        val container = binding.lessonContentContainer

        rawSnippets.forEach { snippet ->
            val button = Button(requireContext()).apply {
                text = snippet.language
                setBackgroundColor(ContextCompat.getColor(context, R.color.bootstrap_green))
                setTextColor(ContextCompat.getColor(context, android.R.color.white))
            }

            val webView = WebView(requireContext()).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MATCH_PARENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                )
                webViewClient = WebViewClient()
                settings.javaScriptEnabled = true
                visibility = View.GONE
            }

            fun prismAlias(lang: String): String {
                return when (lang.lowercase()) {
                    "c++" -> "cpp"
                    "c#" -> "csharp"
                    "c" -> "c"
                    else -> lang.lowercase()
                }
            }

            fun generateHtml(code: String, lang: String): String {
                val escapedCode = code.replace("<", "&lt;").replace(">", "&gt;").replace("\\n", "\n")
                val alias = prismAlias(lang)
                return """
            <html>
            <head>
                <meta charset="utf-8"/>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/themes/prism-tomorrow.min.css"/>
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/line-numbers/prism-line-numbers.min.css"/>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/prism.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/line-numbers/prism-line-numbers.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-c.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-cpp.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-csharp.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-java.min.js"></script>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/components/prism-python.min.js"></script>
                <script>Prism.highlightAll();</script>
            </head>
            <body style="margin:0;padding:16px;background-color:#2d2d2d;color:white;font-family:monospace;">
                <pre class="line-numbers"><code class="language-$alias">$escapedCode</code></pre>
            </body>
            </html>
        """.trimIndent()
            }

            val html = generateHtml(snippet.code, snippet.language)
            webView.loadDataWithBaseURL(null, html, "text/html", "UTF-8", null)

            button.setOnClickListener {
                webView.visibility = if (webView.isVisible) View.GONE else View.VISIBLE
            }

            container.addView(button)
            container.addView(webView)
            addSpacer(container)
        }
    }

    private fun makeTextView(text: String, isTitle: Boolean): TextView {
        return TextView(requireContext()).apply {
            this.text = text
            textSize = if (isTitle) 18f else 16f
            setTextColor(
                if (isTitle) ContextCompat.getColor(context, R.color.bootstrap_green)
                else ContextCompat.getColor(context, android.R.color.black)
            )
            setTypeface(null, if (isTitle) android.graphics.Typeface.BOLD else android.graphics.Typeface.NORMAL)
            setPadding(0, if (isTitle) 24 else 8, 0, 8)
        }
    }

    private fun makeJokeView(question: String, answer: String): View {
        val layout = LinearLayout(requireContext()).apply {
            orientation = LinearLayout.VERTICAL
            setBackgroundColor(ContextCompat.getColor(context, R.color.bootstrap_green))
            setPadding(24, 24, 24, 24)
        }

        val qView = TextView(requireContext()).apply {
            text = question
            setTextColor(ContextCompat.getColor(context, android.R.color.white))
            textSize = 16f
        }

        val aView = TextView(requireContext()).apply {
            text = answer
            setTextColor(ContextCompat.getColor(context, android.R.color.white))
            textSize = 16f
            setPadding(0, 16, 0, 0)
        }

        layout.addView(qView)
        layout.addView(aView)
        return layout
    }

    private fun addSpacer(container: LinearLayout) {
        val space = Space(requireContext())
        space.layoutParams = LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, 32)
        container.addView(space)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
