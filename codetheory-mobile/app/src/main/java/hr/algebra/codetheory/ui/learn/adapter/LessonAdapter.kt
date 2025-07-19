package hr.algebra.codetheory.ui.learn.adapter

import android.view.*
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import hr.algebra.codetheory.R
import hr.algebra.codetheory.model.LessonDto

class LessonAdapter(
    private val lessons: List<LessonDto>,
    private val onClick: (LessonDto) -> Unit
) : RecyclerView.Adapter<LessonAdapter.LessonViewHolder>() {

    class LessonViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val tvLessonTitle: TextView = itemView.findViewById(R.id.tvLessonTitle)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): LessonViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_lesson, parent, false)
        return LessonViewHolder(view)
    }

    override fun onBindViewHolder(holder: LessonViewHolder, position: Int) {
        val lesson = lessons[position]
        holder.tvLessonTitle.text = lesson.title
        holder.itemView.setOnClickListener { onClick(lesson) }
    }

    override fun getItemCount() = lessons.size
}
