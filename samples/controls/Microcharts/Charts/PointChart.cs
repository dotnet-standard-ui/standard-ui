using System;
using System.Collections.Generic;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/BarSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class PointChart : AxisBasedChart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.PointSeriesChart"/> class.
        /// </summary>
        public PointChart(IChart control) : base(control)
        {
        }

        #region Properties

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

        #endregion

        /// <inheritdoc />
        protected override void DrawValueLabel(ICanvas canvas, Dictionary<ChartEntry, Rect> valueLabelSizes, float headerWithLegendHeight, Size itemSize, Size barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            string label = entry?.ValueLabel;
            if (string.IsNullOrEmpty(label))
                return;

            var drawedPoint = new Point(barX - (itemSize.Width / 2) + (barSize.Width / 2), barY);
            var valueLabelOption = Control.ValueLabelOption;
            var valueLabelOrientation = Control.ValueLabelOrientation;
            if (valueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
            else if (valueLabelOption == ValueLabelOption.TopOfElement)
                DrawHelper.DrawLabel(canvas, valueLabelOrientation, valueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementHeight : YPositionBehavior.None, barSize, new Point(drawedPoint.X, drawedPoint.Y - (PointSize / 2) - (Margin / 2)), entry.ValueLabelColor.WithA((byte)(255 * AnimationProgress)), valueLabelSizes[entry], label, Control.ValueLabelTextSize, Typeface);
            else if (valueLabelOption == ValueLabelOption.OverElement)
                DrawHelper.DrawLabel(canvas, valueLabelOrientation, valueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementMiddle : YPositionBehavior.DownToElementMiddle, barSize, new Point(drawedPoint.X, drawedPoint.Y), entry.ValueLabelColor.WithA((byte)(255 * AnimationProgress)), valueLabelSizes[entry], label, Control.ValueLabelTextSize, Typeface);
        }

        /// <inheritdoc />
        protected override void DrawBar(ChartSeries serie, ICanvas canvas, float headerHeight, float itemX, Size itemSize, Size barSize, float origin, float barX, float barY, Color color)
        {
            if (PointMode != PointMode.None)
            {
                var point = new Point(barX - (itemSize.Width / 2) + (barSize.Width / 2), barY);
                canvas.DrawPoint(point, color, PointSize, PointMode);
            }
        }

        /// <inheritdoc />
        protected override void DrawBarArea(ICanvas canvas, float headerHeight, Size itemSize, Size barSize, Color color, float origin, float value, float barX, float barY)
        {
            if (PointAreaAlpha > 0)
            {
                var y = Math.Min(origin, barY);

                using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, barY), new[] { color.WithA(PointAreaAlpha), color.WithA((byte)(PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = color.WithA(PointAreaAlpha),
                })
                {
                    paint.Shader = shader;
                    var height = Math.Max(2, Math.Abs(origin - barY));
                    canvas.DrawRect(SKRect.Create(barX - (itemSize.Width / 2) + (barSize.Width / 2) - (PointSize / 2), y, PointSize, height), paint);
                }
            }
        }
    }
}
