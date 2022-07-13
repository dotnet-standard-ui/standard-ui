// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using static Microsoft.StandardUI.StandardUIStatics;

namespace Microcharts
{
    public interface IPointChart : IChart
    {
    }

    /// <summary>
    /// ![chart](../images/Point.png)
    ///
    /// Point chart.
    /// </summary>
    public class PointChart : ChartBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.PointChart"/> class.
        /// </summary>
        public PointChart(IChart control) : base(control)
        {
            LabelOrientation = Orientation.Horizontal; // Orientation.Default;
            ValueLabelOrientation = Orientation.Default;
        }

        private Orientation labelOrientation, valueLabelOrientation;

        /// <summary>
        /// Gets or sets the size of the point.
        /// </summary>
        /// <value>The size of the point.</value>
        public float PointSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the point mode.
        /// </summary>
        /// <value>The point mode.</value>
        public PointMode PointMode { get; set; } = PointMode.Circle;

        /// <summary>
        /// Gets or sets the point area alpha.
        /// </summary>
        /// <value>The point area alpha.</value>
        public byte PointAreaAlpha { get; set; } = 100;

        /// <summary>
        /// Gets or sets the text orientation of labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <summary>
        /// Gets or sets the text orientation of value labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        private double ValueRange => MaxValue - MinValue;

        public override void BuildContent(ICanvas canvas, int width, int height)
        {
            if (Entries != null)
            {
                var labels = Entries.Select(x => x.Label).ToArray();
                var labelSizes = MeasureLabels(labels);
                var footerHeight = CalculateFooterHeaderHeight(labelSizes, LabelOrientation);

                var valueLabels = Entries.Select(x => x.ValueLabel).ToArray();
                var valueLabelSizes = MeasureLabels(valueLabels);
                var headerHeight = CalculateFooterHeaderHeight(valueLabelSizes, ValueLabelOrientation);
                var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);

                var origin = CalculateYOrigin(itemSize.Height, headerHeight);
                var points = CalculatePoints(itemSize, origin, headerHeight);

                DrawPointAreas(canvas, points, origin);
                BuildPoints(canvas, points);
                BuildHeader(canvas, valueLabels, valueLabelSizes, points, itemSize, height, headerHeight);
                BuildFooter(canvas, labels, labelSizes, points, itemSize, height, footerHeight);
            }
        }

        protected double CalculateYOrigin(double itemHeight, double headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        protected Size CalculateItemSize(int width, int height, double footerHeight, double headerHeight)
        {
            var total = Entries.Count();
            var w = (width - ((total + 1) * Margin)) / total;
            var h = height - Margin - footerHeight - headerHeight;
            return new Size(w, h);
        }

        protected Point[] CalculatePoints(Size itemSize, double origin, double headerHeight)
        {
            var result = new List<Point>();

            for (int i = 0; i < Entries.Count(); i++)
            {
                var entry = Entries.ElementAt(i);
                var value = entry.Value;

                var x = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));
                var y = headerHeight + ((1 - AnimationProgress) * (origin - headerHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);
                var point = new Point(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }

        protected void BuildHeader(ICanvas canvas, string[] labels, Rect[] labelSizes, Point[] points, Size itemSize, int height, double headerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new Point(p.X, headerHeight - Margin)).ToArray(),
                            labelSizes,
                            Entries.Select(x => x.Color.WithA((byte)(255 * AnimationProgress))).ToArray(),
                            ValueLabelOrientation,
                            true,
                            itemSize,
                            height);
        }

        protected void BuildFooter(ICanvas canvas, string[] labels, Rect[] labelSizes, Point[] points, Size itemSize, int height, double footerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new Point(p.X, height - footerHeight + Margin)).ToArray(),
                            labelSizes,
                            Entries.Select(x => LabelColor).ToArray(),
                            LabelOrientation,
                            false,
                            itemSize,
                            height);
        }

        protected void BuildPoints(ICanvas canvas, Point[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, entry.Color, PointSize, PointMode);
                }
            }
        }

        protected void DrawPointAreas(ICanvas canvas, Point[] points, double origin)
        {
            if (points.Length > 0 && PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    var y = Math.Min(origin, point.Y);

                    var color = Color.FromRgb(230, 230, 230);
                    var brush = SolidColorBrush() .Color(color);

                    // TODO: Support gradient bruhes here
#if LATER
                    //using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, point.Y), new[] { entry.Color.WithA(PointAreaAlpha), entry.Color.WithA((byte)(PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color.WithA(PointAreaAlpha),
                    })
#endif
                    var height = Math.Max(2, Math.Abs(origin - point.Y));

                    var barArea = Rectangle() .Width(PointSize) .Height(height) .Fill(brush);
                    canvas.Add(point.X - (PointSize / 2), y, barArea);
                }
            }
        }

        /// <summary>
        /// draws the labels
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="texts"></param>
        /// <param name="points"></param>
        /// <param name="sizes"></param>
        /// <param name="colors"></param>
        /// <param name="orientation"></param>
        /// <param name="isTop"></param>
        /// <param name="itemSize"></param>
        /// <param name="height"></param>
        protected void DrawLabels(ICanvas canvas, string[] texts, Point[] points, Rect[] sizes, Color[] colors, Orientation orientation, bool isTop, Size itemSize, float height)
        {
            if (points.Length > 0)
            {
                var maxWidth = sizes.Max(x => x.Width);

                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];

                    double x, y;

                    if (!string.IsNullOrEmpty(entry.ValueLabel))
                    {
                        /*
                        paint.TextSize = LabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = colors[i];
                        paint.IsStroke = false;
                        paint.Typeface = base.Typeface;
                        */
                        var bounds = sizes[i];
                        var text = texts[i];

                        if (orientation == Orientation.Vertical)
                        {
#if LATER
                            var y = point.Y;

                            if (isTop)
                            {
                                y -= bounds.Width;
                            }

                            canvas.RotateDegrees(90);
                            canvas.Translate(y, -point.X + (bounds.Height / 2));
#endif
                            continue;
                        }
                        else
                        {
#if LATER
                            if (bounds.Width > itemSize.Width)
                            {
                                text = text.Substring(0, Math.Min(3, text.Length));
                                paint.MeasureText(text, ref bounds);
                            }

                            if (bounds.Width > itemSize.Width)
                            {
                                text = text.Substring(0, Math.Min(1, text.Length));
                                paint.MeasureText(text, ref bounds);
                            }
#endif

                            y = point.Y;

                            if (isTop)
                                y -= bounds.Height;

                            x = point.X - (bounds.Width / 2);
                            //canvas.Translate(point.X - (bounds.Width / 2), y);
                        }

                        if (UnicodeMode && !float.TryParse(text, NumberStyles.Any, null, out _))
                        {
#if LATER
                            using (var tf = SKFontManager.Default.MatchCharacter(UnicodeLanguage))
                            using (var shaper = new SKShaper(tf))
                            {
                                canvas.DrawShapedText(shaper, text, 0, 0, paint);
                            }
#endif
                        }
                        else
                        {
                            var textBlock = TextBlock()
                                .Text(text)
                                .Width(100)
                                .Height(LabelTextSize * 2)
                                .Foreground(SolidColorBrush().Color(colors[i]))
                                .FontSize(LabelTextSize);
                            canvas.Add(x, y, textBlock);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the height of the footer.
        /// </summary>
        /// <returns>The footer height.</returns>
        /// <param name="valueLabelSizes">Value label sizes.</param>
        protected double CalculateFooterHeaderHeight(Rect[] valueLabelSizes, Orientation orientation)
        {
            var result = Margin;

            if (Entries.Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                if(orientation == Orientation.Vertical)
                {
                    var maxValueWidth = valueLabelSizes.Max(x => x.Width);
                    if (maxValueWidth > 0)
                    {
                        result += maxValueWidth + Margin;
                    }
                }
                else
                {
                    result += LabelTextSize + Margin;
                }
            }

            return result;
        }

        /// <summary>
        /// Measures the value labels.
        /// </summary>
        /// <returns>The value labels.</returns>
        protected Rect[] MeasureLabels(string[] labels)
        {
#if LATER
            using (var paint = new SKPaint())
            {
                paint.TextSize = LabelTextSize;
                return labels.Select(text =>
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
#endif
            return labels.Select(text => new Rect(0, 0, 50, 50)).ToArray();
        }
    }
}
