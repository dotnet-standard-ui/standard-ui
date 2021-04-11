using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.StandardUI.Drawing
{
    public class DrawingContext
    {
        List<Action<SKCanvas>> actions;
        List<Matrix> totalTransform = new() { Matrix.Identity };

        public DrawingContext(List<Action<SKCanvas>> actions) => this.actions = actions;


        public bool IsVisible { get; private set; }
        public Matrix TotalTransform => totalTransform[totalTransform.Count - 1];


        public void DrawRectangle(Paint paint, Rect rect)
        {
            IsVisible = true;
            actions.Add(canvas =>
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
            });
        }

        public void DrawText(IFormattedText text, Point offset)
        {
            IsVisible = true;
            actions.Add(canvas =>
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
            });
        }

        public IDisposable Push(Matrix transform)
        {
            var newTotal = TotalTransform;
            newTotal *= transform;
            totalTransform.Add(newTotal);
            actions.Add(canvas =>
            {
                canvas.Save();
                var m = transform.Into();
                canvas.Concat(ref m);
            });

            return new Pop(this, totalTransform.Count);
        }

        record Pop(DrawingContext Context, int Count) : IDisposable
        {
            public void Dispose()
            {
                var currentCount = Context.totalTransform.Count;
                Debug.Assert(currentCount == Count, "Unbalanced Push/Pop");
                Context.totalTransform.RemoveAt(currentCount - 1);
                Context.actions.Add(canvas => canvas.Restore());
            }
        }
    }
}
