// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Shapes;
using static Microsoft.StandardUI.StandardUIStatics;
using Microsoft.StandardUI.Media;

namespace Microcharts
{
    public interface IRadarChart : IChart
    {
    }

    /// <summary>
    /// ![chart](../images/Radar.png)
    ///
    /// A radar chart.
    /// </summary>
    public class RadarChart : ChartBase
    {
        private const float Epsilon = 0.01f;

        public RadarChart(IChart control) : base(control)
        {
        }

        /// <summary>
        /// Gets or sets the size of the line.
        /// </summary>
        /// <value>The size of the line.</value>
        public double LineSize { get; set; } = 3;

        /// <summary>
        /// Gets or sets the color of the border line.
        /// </summary>
        /// <value>The color of the border line.</value>
        public Color BorderLineColor { get; set; } = Colors.LightGray.WithA(110);

        /// <summary>
        /// Gets or sets the size of the border line.
        /// </summary>
        /// <value>The size of the border line.</value>
        public double BorderLineSize { get; set; } = 2;

        /// <summary>
        /// Gets or sets the point mode.
        /// </summary>
        /// <value>The point mode.</value>
        public PointMode PointMode { get; set; } = PointMode.Circle;

        /// <summary>
        /// Gets or sets the size of the points.
        /// </summary>
        /// <value>The size of the point.</value>
        public double PointSize { get; set; } = 14;

        private double AbsoluteMinimum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private double AbsoluteMaximum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        private double ValueRange => this.AbsoluteMaximum - this.AbsoluteMinimum;

        public override void BuildContent(ICanvas canvas, int width, int height)
        {
            var total = Entries?.Count() ?? 0;

            if (total > 0)
            {
                var captionHeight = Entries.Max(x =>
                {
                    var result = 0.0;

                    var hasLabel = !string.IsNullOrEmpty(x.Label);
                    var hasValueLabel = !string.IsNullOrEmpty(x.ValueLabel);
                    if (hasLabel || hasValueLabel)
                    {
                        var hasOffset = hasLabel && hasValueLabel;
                        var captionMargin = LabelTextSize * 0.60f;
                        var space = hasOffset ? captionMargin : 0;

                        if (hasLabel)
                        {
                            result += LabelTextSize;
                        }

                        if (hasValueLabel)
                        {
                            result += LabelTextSize;
                        }
                    }

                    return result;
                });

                var center = new Point(width / 2, height / 2);
                var radius = ((Math.Min(width, height) - (2 * Margin)) / 2) - captionHeight;
                var rangeAngle = (float)((Math.PI * 2) / total);
                var startAngle = (float)Math.PI;

                var nextEntry = Entries.First();
                var nextAngle = startAngle;
                var nextPoint = GetPoint(nextEntry.Value * AnimationProgress, center, nextAngle, radius);

                DrawBorder(canvas, center, radius);
#if LATER
                clip.AddCircle(center.X, center.Y, radius);
#endif

                for (int i = 0; i < total; i++)
                {
                    var angle = nextAngle;
                    var entry = nextEntry;
                    var point = nextPoint;

                    var nextIndex = (i + 1) % total;
                    nextAngle = startAngle + (rangeAngle * nextIndex);
                    nextEntry = Entries.ElementAt(nextIndex);
                    nextPoint = GetPoint(nextEntry.Value * AnimationProgress, center, nextAngle, radius);

#if LATER
                    canvas.Save();
                    canvas.ClipPath(clip);
#endif

                    // Border center bars
                    var borderPoint = GetPoint(MaxValue, center, angle, radius);
                    var line = Line() .X1(point.X) .Y1(point.Y) .X2(borderPoint.X) .Y2(borderPoint.Y) .Stroke(SolidColorBrush().Color(BorderLineColor)) .StrokeThickness(BorderLineSize);
                    canvas.Add(0, 0, line);

                    // Values points and lines
                    //PathEffect = SKPathEffect.CreateDash(new[] { BorderLineSize, BorderLineSize * 2 }, 0),

                    var amount = Math.Abs(entry.Value - AbsoluteMinimum) / ValueRange;
                    var diameter = radius * amount * 2;
                    var circleColor = entry.Color.WithA((byte)(entry.Color.A * 0.75f * AnimationProgress));
                    var circle = Ellipse() .Width(diameter) .Height(diameter) .Stroke(SolidColorBrush().Color(circleColor)) .StrokeThickness(BorderLineSize);
                    canvas.Add(center.X - diameter / 2, center.Y - diameter / 2, circle);

                    canvas.DrawGradientLine(center, entry.Color.WithA(0), point, entry.Color.WithA((byte)(entry.Color.A * 0.75f)), LineSize);
                    canvas.DrawGradientLine(point, entry.Color, nextPoint, nextEntry.Color, LineSize);
                    canvas.DrawPoint(point, entry.Color, (float) PointSize, PointMode);
                }

#if LATER
                canvas.Restore();
#endif

#if LATER
                // Labels
                var labelPoint = new Point(0, radius + LabelTextSize + (PointSize / 2));
                var rotation = SKMatrix.MakeRotation(angle);
                labelPoint = center + rotation.MapPoint(labelPoint);
                var alignment = SKTextAlign.Left;

                if ((Math.Abs(angle - (startAngle + Math.PI)) < Epsilon) || (Math.Abs(angle - Math.PI) < Epsilon))
                {
                    alignment = SKTextAlign.Center;
                }
                else if (angle > (float)(startAngle + Math.PI))
                {
                    alignment = SKTextAlign.Right;
                }

                canvas.DrawCaptionLabels(entry.Label, entry.TextColor, UnicodeMode, UnicodeLanguage, entry.ValueLabel, entry.Color.WithA((byte)(255 * AnimationProgress)), LabelTextSize, labelPoint, alignment, base.Typeface, out var _);
#endif
            }
        }

        /// <summary>
        /// Finds point cordinates of an entry.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="value">The value.</param>
        /// <param name="center">The center.</param>
        /// <param name="angle">The entry angle.</param>
        /// <param name="radius">The radius.</param>
        private Point GetPoint(double value, Point center, double angle, double radius)
        {
            var amount = Math.Abs(value - AbsoluteMinimum) / ValueRange;
            var distanceFromOrigin = radius * amount;
            return new Point(center.X + Math.Cos(angle) * distanceFromOrigin, center.Y + Math.Sin(angle) * distanceFromOrigin);
        }

        private void DrawBorder(ICanvas canvas, Point center, double radius)
        {
            var diameter = radius * 2;

            var circle = Ellipse() .Width(diameter) .Height(diameter) .Stroke(SolidColorBrush().Color(BorderLineColor)) .StrokeThickness(BorderLineSize);
            canvas.Add(center.X - radius, center.Y - radius, circle);
        }
    }
}
