// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;

namespace Microcharts
{
    /// <summary>
    /// A chart.
    /// </summary>
    public interface IChart : IStandardControl
    {
        /// <summary>
        /// Chart data
        /// </summary>
        IEnumerable<ChartEntry> Entries { get; set; }

        /// <summary>
        /// Global margin
        /// </summary>
        [DefaultValue(20)]
        float Margin { get; set; }

        /// <summary>
        /// Gets or sets the text size of the labels.
        /// </summary>
        /// <value>The size of the label text.</value>
        [DefaultValue(16)]
        float LabelTextSize { get; set; }

#if TODO
        /// <summary>
        /// Typeface for labels
        /// </summary>
        public SKTypeface Typeface
        {
            get => typeface;
            set => Set(ref typeface, value);
        }
#endif

        /// <summary>
        /// The color of the chart background
        /// </summary>
        [DefaultValue("White")]
        Color BackgroundColor { get; set; }

        /// <summary>
        /// The color of the labels
        /// </summary>
        [DefaultValue("Gray")]
        Color LabelColor { get; set; }

#if TODO
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
#endif

        // Properties for all axis based charts

        private float valueLabelTextSize = 16;
        private float serieLabelTextSize = 16;
        private bool valueLabelTextSizeDefaultValue = true;

        /// <summary>
        /// The legend options for the chart
        /// </summary>
        [DefaultValue(SeriesLegendOption.None)]
        SeriesLegendOption LegendOption { get; set; }

        /// <summary>
        /// The text orientation of labels
        /// </summary>
        [DefaultValue(Orientation.Vertical)]
        Orientation LabelOrientation { get; set; }

        /// <summary>
        /// The text orientation of value labels
        /// </summary>
        [DefaultValue(Orientation.Vertical)]
        Orientation ValueLabelOrientation { get; set; }

        /// <summary>
        /// Gets or sets the text size of the value labels.
        /// </summary>
        /// <value>The size of the value label text.</value>
        public float ValueLabelTextSize
        {
            get => valueLabelTextSize;
            set
            {
                Set(ref valueLabelTextSize, value);
                valueLabelTextSizeDefaultValue = false;
            }
        }

        /// <summary>
        /// Gets or sets the value label layout option
        /// </summary>
        /// <remarks>Default is <seealso cref="T:Microcharts.ValueLabelOption.TopOfChart"/></remarks>
        /// <value>The layout option of value labels</value>
        [DefaultValue(ValueLabelOption.TopOfChart)]
        ValueLabelOption ValueLabelOption { get; set; }

        /// <summary>
        /// Gets or sets the text size of the serie labels.
        /// </summary>
        /// <value>The size of the serie label text.</value>
        [DefaultValue(16)]
        float SeriesLabelTextSize { get; set; }

        /// <summary>
        /// Show Y Axis Text?
        /// </summary>
        [DefaultValue(false)]
        bool ShowYAxisText { get; set; }

        /// <summary>
        /// Show Y Axis Lines?
        /// </summary>
        [DefaultValue(false)]
        bool ShowYAxisLines { get; set; }

        //Microcharts TODO : calculate this automatically, based on available area height and text height
        /// <summary>
        /// Y Axis Max Ticks
        /// </summary>
        [DefaultValue(5)]
        int YAxisMaxTicks { get; set; }

        /// <summary>
        /// Y Axis Position
        /// </summary>
        [DefaultValue(Position.Right)]
        Position YAxisPosition { get; set; }

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisTextPaint { get; set; }

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisLinesPaint { get; set; }
    }
}
