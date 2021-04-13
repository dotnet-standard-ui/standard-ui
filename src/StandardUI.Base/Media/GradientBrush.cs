using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class GradientBrush : Brush
    {
        /// <summary>
        /// A collection of the GradientStop objects associated with the brush, each of which specifies a color and an offset along the brush's gradient axis.
        /// The default is an empty collection.
        /// </summary>
        public IReadOnlyList<GradientStop> GradientStops { get; init; } = Array.Empty<GradientStop>();

        /// <summary>
        /// A BrushMappingMode value that specifies how to interpret the gradient brush's positioning coordinates. The default is RelativeToBoundingBox.
        /// </summary>
        public BrushMappingMode MappingMode { get; init; } = BrushMappingMode.RelativeToBoundingBox;

        /// <summary>
        /// The type of spread method used to paint the gradient. The default is Pad.
        /// </summary>
        public GradientSpreadMethod SpreadMethod { get; init; } = GradientSpreadMethod.Pad;
    }
}
