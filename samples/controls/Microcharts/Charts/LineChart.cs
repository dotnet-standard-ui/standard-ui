using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/LineSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class LineChart : PointChart
    {
        private Dictionary<ChartSeries, List<Point>> pointsPerSeries = new Dictionary<ChartSeries, List<Point>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.LineSeriesChart"/> class.
        /// </summary>
        public LineChart(IChart control) : base(control)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the size of the line.
        /// </summary>
        /// <value>The size of the line.</value>
        public float LineSize { get; set; } = 3;

        /// <summary>
        /// Gets or sets the line mode.
        /// </summary>
        /// <value>The line mode.</value>
        public LineMode LineMode { get; set; } = LineMode.Spline;

        /// <summary>
        /// Gets or sets the alpha of the line area.
        /// </summary>
        /// <value>The line area alpha.</value>
        public byte LineAreaAlpha { get; set; } = 32;

        /// <summary>
        /// Enables or disables a fade out gradient for the line area in the Y direction
        /// </summary>
        /// <value>The state of the fadeout gradient.</value>
        public bool EnableYFadeOutGradient { get; set; } = false;

        #endregion

        /// <inheritdoc/>
        protected override float CalculateHeaderHeight(Dictionary<ChartEntry, Rect> valueLabelSizes)
        {
            if(Control.ValueLabelOption == ValueLabelOption.None || Control.ValueLabelOption == ValueLabelOption.OverElement)
                return Control.Margin;

            return base.CalculateHeaderHeight(valueLabelSizes);
        }

        /// <inheritdoc/>
        public override void DrawContent(ICanvas canvas, int width, int height)
        {
            pointsPerSeries.Clear();
            foreach (var s in Series)
                pointsPerSeries.Add(s, new List<Point>());

            base.DrawContent(canvas, width, height);
        }

        protected override void DrawNullPoint(ChartSeries serie, ICanvas canvas) {
            //Some of the drawing algorithms index into pointsPerSerie
            var point = new Point(float.MinValue, float.MinValue);
            pointsPerSeries[serie].Add(point);
        }

        /// <inheritdoc/>
        protected override void OnDrawContentEnd(ICanvas canvas, Size itemSize, float origin, Dictionary<ChartEntry, Rect> valueLabelSizes)
        {
            base.OnDrawContentEnd(canvas, itemSize, origin, valueLabelSizes);

            foreach (var pps in pointsPerSeries)
            {
                DrawLineArea(canvas, pps.Key, pps.Value.ToArray(), itemSize, origin);
            }

            DrawSeriesLine(canvas, itemSize);
            DrawPoints(canvas);
            DrawValueLabels(canvas, itemSize, valueLabelSizes);
        }

        private void DrawPoints(ICanvas canvas)
        {
            if (PointMode != PointMode.None)
            {
                foreach (var pps in pointsPerSeries)
                {
                    var entries = pps.Key.Entries.ToArray();
                    for (int i = 0; i < pps.Value.Count; i++)
                    {
                        var entry = entries[i];
                        if (!entry.Value.HasValue)
                        {
                            continue;
                        }

                        var point = pps.Value.ElementAt(i);
                        canvas.DrawPoint(point, pps.Key.Color ?? entry.Color, PointSize, PointMode);
                    }
                }
            }
        }

        private void DrawValueLabels(ICanvas canvas, Size itemSize, Dictionary<ChartEntry, Rect> valueLabelSizes)
        {
            ValueLabelOption valueLabelOption = Control.ValueLabelOption;
            if (valueLabelOption == ValueLabelOption.TopOfChart && Series.Count() > 1)
                valueLabelOption = ValueLabelOption.TopOfElement;

            if (valueLabelOption == ValueLabelOption.TopOfElement || valueLabelOption == ValueLabelOption.OverElement)
            {
                foreach (var pps in pointsPerSeries)
                {
                    var entries = pps.Key.Entries.ToArray();
                    for (int i = 0; i < pps.Value.Count; i++)
                    {
                        var entry = entries[i];
                        string label = entry.ValueLabel;
                        if (!string.IsNullOrEmpty(label))
                        {
                            var drawedPoint = pps.Value.ElementAt(i);
                            Point point;
                            YPositionBehavior yPositionBehavior = YPositionBehavior.None;

                            if (!valueLabelSizes.ContainsKey(entry))
                            {
                                continue;
                            }

                            var valueLabelSize = valueLabelSizes[entry];
                            if (valueLabelOption == ValueLabelOption.TopOfElement)
                            {
                                point = new Point(drawedPoint.X, drawedPoint.Y - (PointSize / 2) - (Control.Margin / 2));
                                if (Control.ValueLabelOrientation == Orientation.Vertical)
                                      yPositionBehavior = YPositionBehavior.UpToElementHeight;
                            }
                            else
                            {
                                if (Control.ValueLabelOrientation == Orientation.Vertical)
                                    yPositionBehavior = YPositionBehavior.UpToElementMiddle;
                                else
                                    yPositionBehavior = YPositionBehavior.DownToElementMiddle;

                                point = new Point(drawedPoint.X, drawedPoint.Y);
                            }

                            DrawHelper.DrawLabel(canvas, Control.ValueLabelOrientation, yPositionBehavior, itemSize, point, entry.ValueLabelColor.WithA((byte)(255 * AnimationProgress)), valueLabelSize, label, ValueLabelTextSize, Typeface);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private void DrawSeriesLine(ICanvas canvas, Size itemSize)
        {
            if (pointsPerSeries.Any() && pointsPerSeries.Values.First().Count > 1 && LineMode != LineMode.None)
            {
                foreach (var s in Series)
                {
                    var points = pointsPerSeries[s].ToArray();
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = s.Color ?? SKColors.White,
                        StrokeWidth = LineSize,
                        IsAntialias = true,
                    })
                    {
                        if (s.Color == null)
                            using (var shader = CreateXGradient(points, s.Entries, s.Color))
                                paint.Shader = shader;

                        var path = new SKPath();
                        //path.MoveTo(points.First());

                        var isFirst = true;
                        var entries = s.Entries;
                        var lineMode = LineMode;
                        var last = (lineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
                        for (int i = 0; i < last; i++)
                        {
                            if (!entries.ElementAt(i).Value.HasValue) continue;
                            if (isFirst)
                            {
                                path.MoveTo(points[i]);
                                isFirst = false;
                            }


                            if (lineMode == LineMode.Spline)
                            {
                                int next = i + 1;
                                while (next < last && !entries.ElementAt(next).Value.HasValue)
                                {
                                    next++;
                                }

                                if (next == last && !entries.ElementAt(next).Value.HasValue)
                                {
                                    break;
                                }

                                var cubicInfo = CalculateCubicInfo(points, i, next, itemSize);
                                path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
                            }
                            else if (lineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }

                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        private void DrawLineArea(ICanvas canvas, ChartSeries series, Point[] points, Size itemSize, float origin)
        {
            if (LineAreaAlpha > 0 && points.Length > 1)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.White,
                    IsAntialias = true,
                })
                {
                    using (var shaderX = CreateXGradient(points, series.Entries, series.Color, (byte)(LineAreaAlpha * AnimationProgress)))
                    using (var shaderY = CreateYGradient(points, (byte)(LineAreaAlpha * AnimationProgress)))
                    {
                        paint.Shader = EnableYFadeOutGradient ? SKShader.CreateCompose(shaderY, shaderX, SKBlendMode.SrcOut) : shaderX;

                        var path = new SKPath();

                        var isFirst = true;
                        var entries = series.Entries;
                        var lineMode = LineMode;
                        var last = (lineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
                        SKPoint lastPoint = points.First();
                        for (int i = 0; i < last; i++)
                        {
                            if (!entries.ElementAt(i).Value.HasValue) continue;

                            if( isFirst )
                            {
                                path.MoveTo(points[i].X, origin);
                                path.LineTo(points[i]);
                                isFirst = false;
                            }

                            if (lineMode == LineMode.Spline)
                            {
                                int next = i + 1;
                                while (next < last && !entries.ElementAt(next).Value.HasValue)
                                {
                                    next++;
                                }

                                if (next == last && !entries.ElementAt(next).Value.HasValue)
                                {
                                    lastPoint = points[i];
                                    break;
                                }

                                var cubicInfo = CalculateCubicInfo(points, i, next, itemSize);
                                path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
                                lastPoint = cubicInfo.nextPoint;
                            }
                            else if (lineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                                lastPoint = points[i];
                            }
                        }

                        path.LineTo(lastPoint.X, origin);
                        path.Close();
                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void DrawValueLabel(ICanvas canvas, Dictionary<ChartEntry, Rect> valueLabelSizes, float headerWithLegendHeight, Size itemSize, Size barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            if(Series.Count() == 1 && Control.ValueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
        }

        /// <inheritdoc/>
        protected override void DrawBar(ChartSeries series, ICanvas canvas, float headerHeight, float itemX, Size itemSize, Size barSize, float origin, float barX, float barY, Color color)
        {
            //Drawing entry point at center of the item (label) part
            var point = new Point(itemX, barY);
            pointsPerSeries[series].Add(point);
        }

        /// <inheritdoc/>
        protected override void DrawBarArea(ICanvas canvas, float headerHeight, Size itemSize, Size barSize, Color color, float origin, float value, float barX, float barY)
        {
            //Area is draw on the OnDrawContentEnd
        }

        private (Point control, Point nextPoint, Point nextControl) CalculateCubicInfo(Point[] points, int i, int next, Size itemSize)
        {
            var point = points[i];
            var nextPoint = points[next];
            var controlOffset = new Point(itemSize.Width * 0.8f, 0);
            var currentControl = point + controlOffset;
            var nextControl = nextPoint - controlOffset;
            return (currentControl, nextPoint, nextControl);
        }

        private SKShader CreateXGradient(Point[] points, IEnumerable<ChartEntry> entries, Color? seriesColor, byte alpha = 255)
        {
            var startX = points.First().X;
            var endX = points.Last().X;
            var rangeX = endX - startX;

            return SKShader.CreateLinearGradient(
                new Point(startX, 0),
                new Point(endX, 0),
                entries.Select(x => seriesColor?.WithA(alpha) ?? x.Color.WithA(alpha)).ToArray(),
                null,
                SKShaderTileMode.Clamp);
        }

        private SKShader CreateYGradient(Point[] points, byte alpha = 255)
        {
            var startY = points.Max(i => i.Y);
            var endY = 0;

            return SKShader.CreateLinearGradient(
                new SKPoint(0, startY),
                new SKPoint(0, endY),
                new[] { SKColors.White.WithAlpha(alpha), SKColors.White.WithAlpha(0) },
                null,
                SKShaderTileMode.Clamp);
        }
    }
}
