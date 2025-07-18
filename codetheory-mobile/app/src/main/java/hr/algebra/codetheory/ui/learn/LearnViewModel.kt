package hr.algebra.codetheory.ui.learn

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.viewModelScope
import hr.algebra.codetheory.model.LessonDto
import hr.algebra.codetheory.repository.LessonRepository
import kotlinx.coroutines.launch

class LearnViewModel(application: Application) : AndroidViewModel(application) {

    private val repository = LessonRepository(application.applicationContext)

    private val _lessons = MutableLiveData<List<LessonDto>>()
    val lessons: LiveData<List<LessonDto>> = _lessons

    private val _error = MutableLiveData<String?>()
    val error: LiveData<String?> = _error

    init {
        fetchLessons()
    }

    private fun fetchLessons() {

    }
}
