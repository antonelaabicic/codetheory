package hr.algebra.codetheory.ui.account

import android.content.Intent
import android.os.Bundle
import android.view.*
import android.widget.*
import androidx.fragment.app.Fragment
import com.squareup.picasso.Picasso
import hr.algebra.codetheory.LoginActivity
import hr.algebra.codetheory.R
import hr.algebra.codetheory.api.*
import hr.algebra.codetheory.databinding.FragmentAccountBinding
import hr.algebra.codetheory.framework.TokenManager
import hr.algebra.codetheory.model.UserDto
import hr.algebra.codetheory.model.UserProgressDto
import hr.algebra.codetheory.util.CircleTransform
import kotlinx.coroutines.*

class AccountFragment : Fragment() {

    private var _binding: FragmentAccountBinding? = null
    private val binding get() = _binding!!

    private val userApi by lazy { ApiClient.getApi(UserApi::class.java, requireContext()) }
    private val progressApi by lazy { ApiClient.getApi(UserProgressApi::class.java, requireContext()) }

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        _binding = FragmentAccountBinding.inflate(inflater, container, false)

        binding.logoutButton.setOnClickListener {
            TokenManager.clearToken(requireContext())
            startActivity(Intent(requireContext(), LoginActivity::class.java))
            requireActivity().finish()
        }

        loadUserInfo()
        return binding.root
    }

    private fun loadUserInfo() {
        val username = TokenManager.getUsername(requireContext()) ?: return

        CoroutineScope(Dispatchers.Main).launch {
            try {
                val user = withContext(Dispatchers.IO) {
                    userApi.getUserByUsername(username)
                }
                showUserInfo(user)
                loadProgress(user.id)
            } catch (e: Exception) {
                Toast.makeText(requireContext(), getString(R.string.error_loading_profile), Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun showUserInfo(user: UserDto) {
        Picasso.get()
            .load(user.imagePath)
            .placeholder(R.drawable.neutral_profile)
            .transform(CircleTransform())
            .into(binding.ivProfile)

        setColoredLabelText(binding.tvUsername, "Username: ", user.username)
        setColoredLabelText(binding.tvEmail, "Email: ", user.email)
        setColoredLabelText(binding.tvFullName, "Name: ", "${user.firstName ?: ""} ${user.lastName ?: ""}")
    }

    private fun setColoredLabelText(textView: TextView, label: String, value: String) {
        val fullText = "$label$value"
        val spannable = android.text.SpannableString(fullText)

        val labelColor = android.text.style.ForegroundColorSpan(requireContext().getColor(R.color.bootstrap_green))
        val labelBold = android.text.style.StyleSpan(android.graphics.Typeface.BOLD)

        spannable.setSpan(labelColor, 0, label.length, android.text.Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)
        spannable.setSpan(labelBold, 0, label.length, android.text.Spannable.SPAN_EXCLUSIVE_EXCLUSIVE)

        textView.text = spannable
    }

    private fun loadProgress(userId: Int) {
        CoroutineScope(Dispatchers.Main).launch {
            try {
                val progressList = withContext(Dispatchers.IO) {
                    progressApi.getProgressForUser(userId)
                }
                updateProgressView(progressList)
            } catch (e: Exception) {
                Toast.makeText(requireContext(), getString(R.string.error_loading_progress), Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun updateProgressView(progressList: List<UserProgressDto>) {
        val table = binding.tableProgress
        table.removeViews(1, table.childCount - 1)

        val totalLessons = 5
        val lessonMap = progressList.associateBy { it.lessonId }

        for (i in 1..totalLessons) {
            val progress = lessonMap[i]
            val row = TableRow(requireContext()).apply {
                setPadding(0, 8, 0, 8)
                gravity = Gravity.CENTER_HORIZONTAL
            }

            val lessonTitle = TextView(requireContext()).apply {
                layoutParams = TableRow.LayoutParams(0, ViewGroup.LayoutParams.WRAP_CONTENT, 1f)
                text = getString(R.string.lesson_title, i)
                setTextColor(requireContext().getColor(R.color.black))
                textSize = 16.5f
                gravity = Gravity.CENTER
                textAlignment = View.TEXT_ALIGNMENT_CENTER
            }

            val status = TextView(requireContext()).apply {
                layoutParams = TableRow.LayoutParams(0, ViewGroup.LayoutParams.WRAP_CONTENT, 1f)
                text = when {
                    progress == null -> getString(R.string.lesson_not_started)
                    progress.isCompleted == true -> getString(
                        R.string.lesson_completed,
                        progress.score ?: 0f
                    )

                    else -> getString(R.string.lesson_in_progress, progress.score ?: 0f)
                }

                val score = progress?.score ?: 0f
                setTextColor(
                    when {
                        progress == null -> requireContext().getColor(R.color.purple_700)
                        score < 50f -> requireContext().getColor(R.color.red)
                        else -> requireContext().getColor(R.color.bootstrap_green)
                    }
                )
                textSize = 16.5f
                gravity = Gravity.CENTER
                textAlignment = View.TEXT_ALIGNMENT_CENTER
            }

            row.addView(lessonTitle)
            row.addView(status)
            table.addView(row)
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
