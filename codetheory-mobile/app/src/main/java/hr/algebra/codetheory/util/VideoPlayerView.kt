package hr.algebra.codetheory.util

import android.content.Context
import android.widget.LinearLayout
import android.widget.MediaController
import android.widget.VideoView
import androidx.core.net.toUri

class VideoPlayerView(context: Context) : LinearLayout(context) {

    private val videoView: VideoView = VideoView(context).apply {
        layoutParams = LayoutParams(LayoutParams.MATCH_PARENT, 500)
        val mediaController = MediaController(context)
        setMediaController(mediaController)
        mediaController.setAnchorView(this)
    }

    init {
        addView(videoView)
    }

    fun setVideoUrl(url: String) {
        videoView.setVideoURI(url.toUri())
        videoView.requestFocus()
        videoView.start()
    }
}
