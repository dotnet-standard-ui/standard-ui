// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.StandardUI.Controls;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// A chart.
    /// </summary>
    public abstract class Chart
    {
        /// <summary>
        /// Chart control, where properties are stored
        /// </summary>
        public IChart Control { get; }

        private float? internalMinValue, internalMaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.Chart"/> class.
        /// </summary>
        public Chart(IChart control)
        {
            Control = control;
        }

#if false
        /// <summary>
        /// Gets or sets the minimum value from entries. If not defined, it will be the minimum between zero and the
        /// minimal entry value.
        /// </summary>
        /// <value>The minimum value.</value>
        public float MinValue
        {
            get
            {
                if (!entries.Any())
                {
                    return 0;
                }

                if (InternalMinValue == null)
                {
                    return Math.Min(0, entries.Where( x=>x.Value.HasValue).Min(x => x.Value.Value));
                }

                return Math.Min(InternalMinValue.Value, entries.Where(x => x.Value.HasValue).Min(x => x.Value.Value));
            }

            set => InternalMinValue = value;
        }

        /// <summary>
        /// Gets or sets the maximum value from entries. If not defined, it will be the maximum between zero and the
        /// maximum entry value.
        /// </summary>
        /// <value>The minimum value.</value>
        public float MaxValue
        {
            get
            {
                if (!entries.Any())
                {
                    return 0;
                }

                if (InternalMaxValue == null)
                {
                    return Math.Max(0, entries.Where( x=>x.Value.HasValue ).Max(x => x.Value.Value));
                }

                return Math.Max(InternalMaxValue.Value, entries.Where(x => x.Value.HasValue).Max(x => x.Value.Value));
            }

            set => InternalMaxValue = value;
        }


        /// <summary>
        /// Value range of the chart entries
        /// </summary>
        protected virtual float ValueRange => MaxValue - MinValue;

        /// <summary>
        /// Gets or sets a value whether debug rectangles should be drawn.
        /// </summary>
        internal bool DrawDebugRectangles { get; private set; }
#endif

        /// <summary>
        /// Gets or sets the internal minimum value (that can be null).
        /// </summary>
        /// <value>The internal minimum value.</value>
        protected float? InternalMinValue
        {
            get => internalMinValue;
            set
            {
                if (Set(ref internalMinValue, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                }
            }
        }

        /// <summary>
        /// Gets or sets the internal max value (that can be null).
        /// </summary>
        /// <value>The internal max value.</value>
        protected float? InternalMaxValue
        {
            get => internalMaxValue;
            set
            {
                if (Set(ref internalMaxValue, value))
                {
                    RaisePropertyChanged(nameof(MaxValue));
                }
            }
        }

        /// <summary>
        /// Gets the drawable chart area (is set <see cref="DrawCaptionElements"/>).
        /// This is the total chart size minus the area allocated by caption elements.
        /// </summary>
        protected SKRect DrawableChartArea { get; private set; }

        /// <summary>
        /// Draw the  graph onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Draw(SKCanvas canvas, int width, int height)
        {
            DrawableChartArea = new SKRect(0, 0, width, height);

            // Clear just the drawing area to avoid messing up rest of the canvas in case it's shared
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = BackgroundColor
            })
            {
                canvas.DrawRect(DrawableChartArea, paint);
            }

            DrawContent(canvas, width, height);
        }

        /// <summary>
        /// Draws the chart content.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public abstract void DrawContent(SKCanvas canvas, int width, int height);

        /// <summary>
        /// Draws caption elements on the right or left side of the chart.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="entries">The entries.</param>
        /// <param name="isLeft">If set to <c>true</c> is left.</param>
        /// <param name="isGraphCentered">Should the chart in the center always?</param>
        protected void DrawCaptionElements(SKCanvas canvas, int width, int height, List<ChartEntry> entries,
            bool isLeft, bool isGraphCentered)
        {
            var totalMargin = 2 * Control.Margin;
            var availableHeight = height - (2 * totalMargin);
            var x = isLeft ? Control.Margin : (width - Control.Margin - Control.LabelTextSize);
            var ySpace = (availableHeight - Control.LabelTextSize) / ((entries.Count <= 1) ? 1 : entries.Count - 1);

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries.ElementAt(i);
                var y = totalMargin + (i * ySpace);
                if (entries.Count <= 1)
                {
                    y += (availableHeight - Control.LabelTextSize) / 2;
                }

                var hasLabel = !string.IsNullOrEmpty(entry.Label);
                var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

                if (hasLabel || hasValueLabel)
                {
                    var captionMargin = Control.LabelTextSize * 0.60f;
                    var captionX = isLeft ? Control.Margin : width - Control.Margin - Control.LabelTextSize;
                    var legendColor = entry.Color.WithA((byte)(entry.Color.A * AnimationProgress));
                    var valueColor =
                        entry.ValueLabelColor.WithA((byte)(entry.ValueLabelColor.A * AnimationProgress));
                    var lblColor = entry.TextColor.WithA((byte)(entry.TextColor.A * AnimationProgress));
                    var rect = SKRect.Create(captionX, y, Control.LabelTextSize, Control.LabelTextSize);

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = legendColor
                    })
                    {
                        canvas.DrawRect(rect, paint);
                    }

                    if (isLeft)
                    {
                        captionX += LabelTextSize + captionMargin;
                    }
                    else
                    {
                        captionX -= captionMargin;
                    }

                    canvas.DrawCaptionLabels(entry.Label, lblColor, entry.ValueLabel, valueColor, Control.LabelTextSize,
                        new Point(captionX, y + (Control.LabelTextSize / 2)), isLeft ? SKTextAlign.Left : SKTextAlign.Right,
                        Typeface, out var labelBounds);
                    labelBounds.Union(rect);

                    if (DrawDebugRectangles)
                    {
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = entry.Color,
                            IsStroke = true
                        })
                        {
                            canvas.DrawRect(labelBounds, paint);
                        }
                    }

                    if (isLeft)
                    {
                        DrawableChartArea = new SKRect(Math.Max(DrawableChartArea.Left, labelBounds.Right), 0,
                            DrawableChartArea.Right, DrawableChartArea.Bottom);
                    }
                    else
                    {
                        // Draws the chart centered for right labelmode only
                        var left = isGraphCentered == true ? Math.Abs(width - DrawableChartArea.Right) : 0;
                        DrawableChartArea = new SKRect(left, 0, Math.Min(DrawableChartArea.Right, labelBounds.Left),
                            DrawableChartArea.Bottom);
                    }

                    if (entries.Count == 0 && isGraphCentered)
                    {
                        // Draws the chart centered if there isn't any left values
                        DrawableChartArea = new SKRect(Math.Abs(width - DrawableChartArea.Right), 0,
                            DrawableChartArea.Right, DrawableChartArea.Bottom);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a weak event handler to observe invalidate changes.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <param name="onInvalidate">Callback when chart is invalidated.</param>
        /// <typeparam name="TTarget">The target subsriber type.</typeparam>
        public InvalidatedWeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target,
            Action<TTarget> onInvalidate)
            where TTarget : class
        {
            var weakHandler = new InvalidatedWeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subsribe();
            return weakHandler;
        }
    }
}
