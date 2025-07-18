package hr.algebra.codetheory.ui.learn

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import hr.algebra.codetheory.databinding.FragmentLearnBinding

class LearnFragment : Fragment() {

    private var _binding: FragmentLearnBinding? = null
    private val binding get() = _binding!!

    private lateinit var learnViewModel: LearnViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLearnBinding.inflate(inflater, container, false)
//        learnViewModel = ViewModelProvider(this)[LearnViewModel::class.java]
//
//        learnViewModel.lessons.observe(viewLifecycleOwner) { lessons ->
//            lessons.forEach {
//                Log.d("LESSON", "ID: ${it.id}, Title: ${it.title}, Summary: ${it.summary}")
//            }
//        }
//
//        learnViewModel.error.observe(viewLifecycleOwner) { errorMsg ->
//            errorMsg?.let { Log.e("API_ERROR", it) }
//        }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
