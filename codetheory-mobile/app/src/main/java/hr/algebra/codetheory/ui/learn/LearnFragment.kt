package hr.algebra.codetheory.ui.learn

import android.os.Bundle
import android.view.*
import android.util.Log
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import hr.algebra.codetheory.R
import hr.algebra.codetheory.databinding.FragmentLearnBinding
import hr.algebra.codetheory.model.LessonDto
import hr.algebra.codetheory.ui.learn.adapter.LessonAdapter

class LearnFragment : Fragment() {

    private var _binding: FragmentLearnBinding? = null
    private val binding get() = _binding!!

    private lateinit var viewModel: LearnViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLearnBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(this)[LearnViewModel::class.java]

        binding.recyclerView.layoutManager = LinearLayoutManager(requireContext())

        viewModel.lessons.observe(viewLifecycleOwner) { lessons ->
            binding.recyclerView.adapter = LessonAdapter(lessons) { lesson ->
                val bundle = Bundle().apply {
                    putInt("lessonId", lesson.id)
                }
                val fragment = LessonDetailFragment()
                fragment.arguments = bundle

                findNavController().navigate(
                    R.id.action_navigation_learn_to_lessonDetailFragment,
                    bundle
                )
            }
        }

        viewModel.error.observe(viewLifecycleOwner) { errorMsg ->
            errorMsg?.let {
                Log.e("ERROR", it)
            }
        }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
