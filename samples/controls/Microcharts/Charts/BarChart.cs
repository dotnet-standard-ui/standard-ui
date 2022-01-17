// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.StandardUI;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/BarSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class BarChart : AxisBasedChart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.BarSeriesChart"/> class.
        /// </summary>
        public BarChart(IChart control) : base(control)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the bar background area alpha.
        /// </summary>
        /// <value>The bar area alpha.</value>
        public byte BarAreaAlpha { get; set; } = DefaultValues.BarAreaAlpha;

        /// <summary>
        /// Get or sets the minimum height for a bar
        /// </summary>
        /// <value>The minium height of a bar.</value>
        public float MinBarHeight { get; set; } = DefaultValues.MinBarHeight;

        #endregion

        /// <inheritdoc/>
        protected override float CalculateHeaderHeight(Dictionary<ChartEntry, Rect> valueLabelSizes)
        {
            if (Control.ValueLabelOption == ValueLabelOption.None || Control.ValueLabelOption == ValueLabelOption.OverElement)
                return Control.Margin;

            return base.CalculateHeaderHeight(valueLabelSizes);
        }

        /// <inheritdoc/>
        protected override void DrawValueLabel(SKCanvas canvas, Dictionary<ChartEntry, Rect> valueLabelSizes, float headerWithLegendHeight, Size itemSize, Size barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            if (string.IsNullOrEmpty(entry?.ValueLabel))
                return;

            (Point location, Size size) = GetBarDrawingProperties(headerWithLegendHeight, itemSize, barSize, 0, barX, barY);
            if (Control.ValueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
            else if (Control.ValueLabelOption == ValueLabelOption.TopOfElement)
                DrawHelper.DrawLabel(canvas, Control.ValueLabelOrientation, ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementHeight : YPositionBehavior.None, barSize, new SKPoint(location.X + size.Width / 2, barY - Margin), entry.ValueLabelColor.WithA((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
            else if (Control.ValueLabelOption == ValueLabelOption.OverElement)
                DrawHelper.DrawLabel(canvas, Control.ValueLabelOrientation, Control.ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementMiddle : YPositionBehavior.DownToElementMiddle, barSize, new Point(location.X + size.Width / 2, barY + (origin - barY) / 2), entry.ValueLabelColor.WithA((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, Control.ValueLabelTextSize, Typeface);
        }

        /// <inheritdoc />
        protected override void DrawBar(ChartSeries serie, SKCanvas canvas, float headerHeight, float itemX, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = color,
            })
            {
                (SKPoint location, SKSize size) = GetBarDrawingProperties(headerHeight, itemSize, barSize, origin, barX, barY);
                var rect = SKRect.Create(location, size);
                canvas.DrawRect(rect, paint);
            }
        }

        private (Point location, Size size) GetBarDrawingProperties(float headerHeight, Size itemSize, Size barSize, float origin, float barX, float barY)
        {
            double x = barX - (itemSize.Width / 2);
            double y = Math.Min(origin, barY);
            var height = Math.Max(MinBarHeight, Math.Abs(origin - barY));
            if (height < MinBarHeight)
            {
                height = MinBarHeight;
                if (y + height > Control.Margin + itemSize.Height)
                {
                    y = headerHeight + itemSize.Height - height;
                }
            }

            return (new Point(x, y), new Size(barSize.Width, height));
        }

        /// <inheritdoc />
        protected override void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float origin, float value, float barX, float barY)
        {
            if (BarAreaAlpha > 0)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = color.WithA((byte)(this.BarAreaAlpha * this.AnimationProgress)),
                })
                {
                    var max = value > 0 ? headerHeight : headerHeight + itemSize.Height;
                    var height = Math.Abs(max - barY);
                    var y = Math.Min(max, barY);
                    canvas.DrawRect(SKRect.Create(barX - (itemSize.Width / 2), y, barSize.Width, height), paint);
                }
            }
        }
    }
}
