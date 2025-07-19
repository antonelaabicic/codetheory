package hr.algebra.codetheory.ui.learn

import android.app.Application
import androidx.lifecycle.*
import hr.algebra.codetheory.api.ApiClient
import hr.algebra.codetheory.api.LessonApi
import hr.algebra.codetheory.model.LessonDto
import kotlinx.coroutines.launch

class LearnViewModel(application: Application) : AndroidViewModel(application) {

    private val lessonApi = ApiClient.getApi(LessonApi::class.java, application.applicationContext)

    private val _lessons = MutableLiveData<List<LessonDto>>()
    val lessons: LiveData<List<LessonDto>> = _lessons

    private val _error = MutableLiveData<String?>()
    val error: LiveData<String?> = _error

    init {
        fetchLessons()
    }

    private fun fetchLessons() {
        viewModelScope.launch {
            try {
                val response = lessonApi.getAllLessons()
                _lessons.value = response
            } catch (e: Exception) {
                _error.value = e.message
            }
        }
    }
}
