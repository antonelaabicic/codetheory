package hr.algebra.codetheory.ui.learn

import android.os.Bundle
import android.util.Log
import android.view.*
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.google.gson.Gson
import com.google.gson.JsonSyntaxException
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.LessonApi
import hr.algebra.codetheory.databinding.FragmentLessonDetailBinding
import hr.algebra.codetheory.model.*
import kotlinx.coroutines.launch

class LessonDetailFragment : Fragment() {

    private var _binding: FragmentLessonDetailBinding? = null
    private val binding get() = _binding!!

    private val lessonApi by lazy {
        ApiClient.getApi(LessonApi::class.java, requireContext())
    }

    private val gson = Gson()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLessonDetailBinding.inflate(inflater, container, false)
        val lessonId = requireArguments().getInt("lessonId")
        fetchLessonDetail(lessonId)
        return binding.root
    }

    private fun fetchLessonDetail(id: Int) {
        lifecycleScope.launch {
            try {
                val lesson = lessonApi.getLesson(id)

                (requireActivity() as AppCompatActivity).supportActionBar?.title = lesson.title
                binding.lessonTitleText.text = lesson.title

                lesson.contents.forEach { content ->
                    if (content.contentTypeId == 1) {
                        parseAndDisplayTextContent(content.contentData)
                    }
                }

            } catch (e: Exception) {
                Log.e("LESSON_DETAIL_ERROR", "Failed to fetch lesson: ${e.message}")
            }
        }
    }

    private fun parseAndDisplayTextContent(json: String) {
        try {
            val container = binding.lessonContentContainer

            val joke = runCatching {
                gson.fromJson(json, JokeContentDataDto::class.java)
            }.getOrNull()

            if (joke?.question != null && joke.answer != null) {
                container.addView(makeTextView("${joke.question}", true))
                container.addView(makeTextView("${joke.answer}", false))
                return
            }
            
            val text = gson.fromJson(json, TextContentDataDto::class.java)

            text.title?.let {
                container.addView(makeTextView(it, true))
            }

            text.text?.let {
                container.addView(makeTextView(it, false))
            }

            text.bullets?.let { bulletList: List<BulletDto> ->
                bulletList.forEach { bullet ->
                    val heading = "• ${bullet.heading}"
                    val bulletText = bullet.text
                    container.addView(makeTextView(heading, true))
                    container.addView(makeTextView(bulletText, false))
                }
            }

        } catch (e: JsonSyntaxException) {
            Log.e("PARSE_ERROR", "Invalid JSON: $json", e)
        }
    }

    private fun makeTextView(text: String, isTitle: Boolean): TextView {
        val tv = TextView(requireContext())
        tv.text = text
        tv.textSize = if (isTitle) 18f else 16f
        tv.setPadding(0, if (isTitle) 16 else 8, 0, 8)
        return tv
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
