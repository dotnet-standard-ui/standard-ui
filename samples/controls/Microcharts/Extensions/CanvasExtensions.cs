// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using Microsoft.StandardUI.Text;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using static Microsoft.StandardUI.StandardUIStatics;

namespace Microcharts
{
    internal static class CanvasExtensions
    {
        public static void BuildCaptionLabels(this ICanvas canvas, string label, Color labelColor, bool labelIsUnicode, char unicodeLang, string value, Color valueColor, double textSize, Point point, TextAlignment horizontalAlignment, SKTypeface typeface, out Rect totalBounds)
        {
            var hasLabel = !string.IsNullOrEmpty(label);
            var hasValueLabel = !string.IsNullOrEmpty(value);

            totalBounds = new Rect();

            if (hasLabel || hasValueLabel)
            {
                var hasOffset = hasLabel && hasValueLabel;
                var captionMargin = textSize * 0.60f;
                var space = hasOffset ? captionMargin : 0;

                if (hasLabel)
                {
                    // TODO: Add support for typeface
                    var labelTextBlock = TextBlock()
                        .Text(label)
                        .FontSize(textSize)
                        .Foreground(SolidColorBrush().Color(labelColor))
                        .TextAlignment(horizontalAlignment);


                    labelTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Size labelSize = labelTextBlock.DesiredSize;

                    //var bounds = new Rect();
                    //paint.MeasureText(text, ref bounds);

                    var y = point.Y - (labelSize.Height / 2) - space;

                    //canvas.DrawText(text, point.X, y, paint);
                    canvas.Add(point.X, y, labelTextBlock);

                    //Rect labelBounds = GetAbsolutePositionRect(point.X, y, labelSize, horizontalAlignment);
                    //totalBounds = labelBounds.Standardized;
                }

                if (hasValueLabel)
                {
                    // TODO: Add support for typeface
                    var valueTextBlock = TextBlock()
                        .Text(value)
                        .FontSize(textSize)
                        .FontWeight(FontWeights.Bold)
                        .Foreground(SolidColorBrush().Color(valueColor))
                        .TextAlignment(horizontalAlignment);

                    valueTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Size valueSize = valueTextBlock.DesiredSize;

                    //var bounds = new Rect();
                    //paint.MeasureText(text, ref bounds);

                    var y = point.Y - (valueSize.Height / 2) + space;

                    canvas.Add(point.X, y, valueTextBlock);

#if LATER
                    var valueBounds = GetAbsolutePositionRect(point.X, y, bounds, horizontalAlignment);
                    if (totalBounds.IsEmpty)
                    {
                        totalBounds = valueBounds;
                    }
                    else
                    {
                        totalBounds.Union(valueBounds);
                    }
#endif
                }
            }
        }

        /// <summary>
        /// Draws the given point.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="point">The point.</param>
        /// <param name="color">The fill color.</param>
        /// <param name="size">The point size.</param>
        /// <param name="mode">The point mode.</param>
        public static void DrawPoint(this ICanvas canvas, Point point, Color color, float size, PointMode mode)
        {
            IShape shape;
            if (mode == PointMode.Square)
                shape = Rectangle();
            else if (mode == PointMode.Circle)
                shape = Ellipse();
            else return;

            canvas.Add(point.X - (size / 2), point.Y - (size / 2), shape.Width(size).Height(size).Fill(SolidColorBrush().Color(color)));
        }

        /// <summary>
        /// Draws a line with a gradient stroke.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="startPoint">The starting point.</param>
        /// <param name="startColor">The starting color.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="size">The stroke size.</param>
        public static void DrawGradientLine(this ICanvas canvas, Point startPoint, Color startColor, Point endPoint, Color endColor, double size)
        {
#if false
            using (var shader = SKShader.CreateLinearGradient(startPoint, endPoint, new[] { startColor, endColor }, null, SKShaderTileMode.Clamp))
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = size,
                    Shader = shader,
                    IsAntialias = true,
                })
                {
                    canvas.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, paint);
                }
            }
#endif
        }

#if LATER
        /// <summary>
        /// Gets the absolute bounds of a given rectangle, aligned at a given position.
        /// </summary>
        /// <param name="x">The absolute x position.</param>
        /// <param name="y">The absolute y position.</param>
        /// <param name="bounds">The bounds of the rectangle.</param>
        /// <param name="horizontalAlignment">The alignment of the rectangle, relative to x/y.</param>
        /// <returns></returns>
        private static Rect GetAbsolutePositionRect(double x, double y, Rect bounds, TextAlignment horizontalAlignment)
        {
            var captionBounds = new Rect
            {
                X = x + bounds.Left,
                Y = y + bounds.Top
            };

            // TODO: This logic doesn't seem right
            switch (horizontalAlignment)
            {
                case TextAlignment.Left:
                    captionBounds.Width = bounds.Width;
                    break;
                case TextAlignment.Center:
                    captionBounds.Width = bounds.Width / 2;
                    break;
                case TextAlignment.Right:
                    captionBounds.Width = -bounds.Width;
                    break;
            }

            captionBounds.Height = bounds.Height;

            return captionBounds;
        }
#endif
    }
}
