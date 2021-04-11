using SkiaSharp;
using System;
using System.Diagnostics;

namespace Microsoft.StandardUI.Drawing
{
    public class DrawingContext
    {
        private SKCanvas canvas;

        public DrawingContext(SKCanvas canvas) => this.canvas = canvas;

        public Matrix TotalTransform => canvas.TotalMatrix.Into();

        public void DrawRectangle(Paint paint, Rect rect)
        {
            var paints = paint.Into();
            foreach (var p in paints)
            {
                var current = rect.Into();
                if (p.StrokeWidth > 0)
                {
                    var v = p.StrokeWidth * 0.5f;
                    current.Left += v;
                    current.Top += v;
                    current.Right -= v;
                    current.Bottom -= v;
                }
                canvas.DrawRect(current, p);
            }
        }

        public void DrawText(IFormattedText text, Point offset)
        {
            var txt = (FormattedText)text;
            var sx = canvas.TotalMatrix.ScaleX;
            var sy = canvas.TotalMatrix.ScaleY;
            canvas.Save();
            try
            {
                canvas.Scale(1 / sx, 1 / sy);
                canvas.DrawText(txt.Blob, 0, 0, txt.Paint);
            }
            finally
            {
                canvas.Restore();
            }
        }

        public IDisposable Push(Matrix transform)
        {
            int saveCount = canvas.Save();

            var t = transform.Into();
            canvas.Concat(ref t);
            return new Pop(canvas, saveCount);
        }

        private record Pop(SKCanvas Canvas, int Count) : IDisposable
        {
            public void Dispose()
            {
                Canvas.Restore();
                Debug.Assert(Canvas.SaveCount == Count, "Unbalanced Push/Pop");
            }
        }
    }
}
