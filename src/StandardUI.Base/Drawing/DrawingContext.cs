using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.StandardUI.Drawing
{
    public class DrawingContext
    {
        SKCanvas canvas;
        List<Matrix> totalTransform = new() { Matrix.Identity };

        public DrawingContext(SKCanvas canvas) => this.canvas = canvas;

        public bool IsVisible { get; private set; }
        public Matrix TotalTransform => totalTransform[totalTransform.Count - 1];

        public void DrawEllipse(Ellipse ellipse, Size size)
        {
            SKPath skPath = new SKPath();
            SKRect skRect = SKRect.Create(0, 0, size.Width, size.Height);
            skPath.AddOval(skRect);

            DrawShapePath(skPath, ellipse);
        }

        public void DrawLine(Line line)
        {
            SKPath skPath = new SKPath();
            skPath.MoveTo(line.X1, line.Y1);
            skPath.LineTo(line.X2, line.Y2);

            DrawShapePath(skPath, line);
        }

        public void DrawPath(Path path)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygon(Polygon polygon)
        {
            SKPath skPath = new SKPath();
            skPath.FillType = FillRuleToSkiaPathFillType(polygon.FillRule);
            skPath.AddPoly(PointsToSkiaPoints(polygon.Points), close: true);

            DrawShapePath(skPath, polygon);
        }

        public void DrawPolyline(Polyline polyline)
        {
            SKPath skPath = new SKPath();
            skPath.FillType = FillRuleToSkiaPathFillType(polyline.FillRule);
            skPath.AddPoly(PointsToSkiaPoints(polyline.Points), close: false);

            DrawShapePath(skPath, polyline);
        }

        public void DrawRectangle(Rectangle rectangle, Size size)
        {
            SKPath skPath = new SKPath();
            SKRect skRect = SKRect.Create(0, 0, size.Width, size.Height);
            if (rectangle.RadiusX > 0 || rectangle.RadiusY > 0)
                skPath.AddRoundRect(skRect, (float)rectangle.RadiusX, (float)rectangle.RadiusY);
            else
                skPath.AddRect(skRect);

            DrawShapePath(skPath, rectangle);
        }

        private void DrawShapePath<TShape>(SKPath skPath, TShape shape) where TShape : Shape<TShape>
        {
            FillSkiaPath(skPath, shape);
            StrokeSkiaPath(skPath, shape);
        }

        private void FillSkiaPath<TShape>(SKPath skPath, TShape shape) where TShape : Shape<TShape>
        {
            Microsoft.StandardUI.Media.Brush? fill = shape.Fill;
            if (fill != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Fill, IsAntialias = true };
                InitSkiaPaintForBrush(paint, fill, shape);
                canvas.DrawPath(skPath, paint);
            }
        }

        private void StrokeSkiaPath<TShape>(SKPath skPath, TShape shape) where TShape : Shape<TShape>
        {
            Microsoft.StandardUI.Media.Brush? stroke = shape.Stroke;
            if (stroke != null)
            {
                using SKPaint paint = new SKPaint { Style = SKPaintStyle.Stroke, IsAntialias = true };
                InitSkiaPaintForBrush(paint, stroke, shape);
                paint.StrokeWidth = (int)shape.StrokeThickness;
                paint.StrokeMiter = (float)shape.StrokeMiterLimit;

                SKStrokeCap strokeCap = shape.StrokeLineCap switch
                {
                    PenLineCap.Flat => SKStrokeCap.Butt,
                    PenLineCap.Round => SKStrokeCap.Round,
                    PenLineCap.Square => SKStrokeCap.Square,
                    _ => throw new InvalidOperationException($"Unknown PenLineCap value {shape.StrokeLineCap}")
                };
                paint.StrokeCap = strokeCap;

                SKStrokeJoin strokeJoin = shape.StrokeLineJoin switch
                {
                    PenLineJoin.Miter => SKStrokeJoin.Miter,
                    PenLineJoin.Bevel => SKStrokeJoin.Bevel,
                    PenLineJoin.Round => SKStrokeJoin.Round,
                    _ => throw new InvalidOperationException($"Unknown PenLineJoin value {shape.StrokeLineJoin}")
                };
                paint.StrokeJoin = strokeJoin;

                canvas.DrawPath(skPath, paint);
            }
        }

        private static void InitSkiaPaintForBrush<TShape>(SKPaint paint, Microsoft.StandardUI.Media.Brush brush, TShape shape) where TShape : Shape<TShape>
        {
            if (brush is Microsoft.StandardUI.Media.SolidColorBrush solidColorBrush)
                paint.Color = ToSkiaColor(solidColorBrush.Color);
            else if (brush is GradientBrush gradientBrush)
                paint.Shader = ToSkiaShader(gradientBrush, shape);
            else throw new InvalidOperationException($"Brush type {brush.GetType()} isn't currently supported");
        }

        private static SKColor ToSkiaColor(Color color) => new SKColor(color.Red, color.Green, color.Blue, color.Alpha);

        private static SKShader ToSkiaShader<TShape>(GradientBrush gradientBrush, TShape shape) where TShape : Shape<TShape>
        {
            SKShaderTileMode tileMode = gradientBrush.SpreadMethod switch
            {
                GradientSpreadMethod.Pad => SKShaderTileMode.Clamp,
                GradientSpreadMethod.Reflect => SKShaderTileMode.Mirror,
                GradientSpreadMethod.Repeat => SKShaderTileMode.Repeat,
                _ => throw new InvalidOperationException($"Unknown GradientSpreadMethod value {gradientBrush.SpreadMethod}")
            };

            List<SKColor> skColors = new List<SKColor>();
            List<float> skiaColorPositions = new List<float>();
            foreach (GradientStop gradientStop in gradientBrush.GradientStops)
            {
                skColors.Add(ToSkiaColor(gradientStop.Color));
                skiaColorPositions.Add((float)gradientStop.Offset);
            }

            if (gradientBrush is LinearGradientBrush linearGradientBrush)
            {
                SKPoint skiaStartPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.StartPoint, gradientBrush, shape);
                SKPoint skiaEndPoint = GradientBrushPointToSkiaPoint(linearGradientBrush.EndPoint, gradientBrush, shape);

                return SKShader.CreateLinearGradient(skiaStartPoint, skiaEndPoint, skColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else if (gradientBrush is RadialGradientBrush radialGradientBrush)
            {
                SKPoint skiaCenterPoint = GradientBrushPointToSkiaPoint(radialGradientBrush.Center, gradientBrush, shape);

                // TODO: Support auto width/height properly
                float radius = radialGradientBrush.RadiusX * shape.NaturalSize.Width;
                return SKShader.CreateRadialGradient(skiaCenterPoint, radius, skColors.ToArray(), skiaColorPositions.ToArray(), tileMode);
            }
            else throw new InvalidOperationException($"GradientBrush type {gradientBrush.GetType()} is unknown");
        }

        private static SKPoint GradientBrushPointToSkiaPoint<TShape>(Point point, GradientBrush gradientBrush, TShape shape) where TShape : Shape<TShape>
        {
            if (gradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox) {
                // TODO: Support auto width/height properly
                Size naturalSize = shape.NaturalSize;
                return new SKPoint(
                    point.X * naturalSize.Width,
                    point.Y * naturalSize.Height);
            }
            else
                return new SKPoint(point.X, point.Y);
        }

        private static SKPathFillType FillRuleToSkiaPathFillType(FillRule fillRule)
        {
            return fillRule switch
            {
                FillRule.EvenOdd => SKPathFillType.EvenOdd,
                FillRule.Nonzero => SKPathFillType.Winding,
                _ => throw new InvalidOperationException($"Unknown fillRule value {fillRule}")
            };
        }

        private static SKPoint[] PointsToSkiaPoints(Points points)
        {
            int length = points.Length;
            SKPoint[] skiaPoints = new SKPoint[length];
            for (int i = 0; i < length; i++)
                skiaPoints[i] = new SKPoint(points[i].X, points[i].Y);

            return skiaPoints;
        }

        public void DrawRectangle(Paint paint, Rect rect)
        {
            IsVisible = true;

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
            IsVisible = true;

            var txt = (FormattedText)text;
            var invScale = 1 / txt.Scale;
            canvas.Save();
            try
            {
                canvas.Scale(invScale, invScale);
                canvas.DrawText(txt.Blob, 0, 0, txt.Paint);
            }
            finally
            {
                canvas.Restore();
            }
        }

        public IDisposable Push(Matrix transform)
        {
            var newTotal = TotalTransform;
            newTotal *= transform;
            totalTransform.Add(newTotal);

                canvas.Save();
                var m = transform.Into();
                canvas.Concat(ref m);

            return new Pop(this, totalTransform.Count);
        }

        record Pop(DrawingContext Context, int Count) : IDisposable
        {
            public void Dispose()
            {
                var currentCount = Context.totalTransform.Count;
                Debug.Assert(currentCount == Count, "Unbalanced Push/Pop");
                Context.totalTransform.RemoveAt(currentCount - 1);
                Context.canvas.Restore();
            }
        }
    }
}
