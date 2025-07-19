package hr.algebra.codetheory.util

import android.graphics.*
import com.squareup.picasso.Transformation
import androidx.core.graphics.createBitmap

class CircleTransform : Transformation {
    override fun transform(source: Bitmap): Bitmap {
        val size = minOf(source.width, source.height)
        val x = (source.width - size) / 2
        val y = (source.height - size) / 2

        val squared = Bitmap.createBitmap(source, x, y, size, size)
        if (squared != source) source.recycle()

        val result = createBitmap(size, size)

        val canvas = Canvas(result)
        val paint = Paint()
        paint.shader = BitmapShader(squared, Shader.TileMode.CLAMP, Shader.TileMode.CLAMP)
        paint.isAntiAlias = true

        val r = size / 2f
        canvas.drawCircle(r, r, r, paint)

        squared.recycle()
        return result
    }

    override fun key(): String = "circle"
}
