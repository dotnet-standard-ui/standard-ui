using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Media;
using System;

namespace Microsoft.StandardUI.Shapes
{
    // TODO: Add superclass
    public abstract class Shape<Self> : DrawingElement<Self> where Self : Shape<Self>
    {
        /// <summary>
        /// A Brush that paints/fills the shape interior. The default is null (a null brush) which is evaluated as Transparent for rendering.
        /// </summary>
        public Brush? Fill { get; init; } = null;

        /// <summary>
        /// A Brush that specifies how the Shape outline is painted. The default is null.
        /// </summary>
        public Brush? Stroke { get; init; } = null;

        /// <summary>
        /// The width of the Shape outline, in pixels. The default value is 0.
        /// </summary>
        public float StrokeThickness { get; init; } = 0;

        /// <summary>
        /// The limit on the ratio of the miter length to the StrokeThickness of a Shape element. This value is always a positive number that is greater than or equal to 1.
        /// </summary>
        public float StrokeMiterLimit { get; init; } = 10;

        /// <summary>
        /// A value of the PenLineCap enumeration that specifies the shape at the start of a Stroke. The default is Flat.
        /// </summary>
        public PenLineCap StrokeLineCap { get; init; } = PenLineCap.Flat;

        /// <summary>
        /// A value of the PenLineJoin enumeration that specifies the join appearance. The default value is Miter.
        /// </summary>
        public PenLineJoin StrokeLineJoin { get; init; } = PenLineJoin.Miter;

        public virtual Size NaturalSize => throw new NotImplementedException();

        public override Size Arrange(Size availableSize)
        {
            Size naturalSize = NaturalSize;
            return new Size(
                GetSpecifiedOrDefault(naturalSize.Width, availableSize.Width),
                GetSpecifiedOrDefault(naturalSize.Height, availableSize.Height)
            );
        }

        public static float GetSpecifiedOrDefault(float specifiedValue, float defaultValue) =>
            float.IsNaN(specifiedValue) || float.IsPositiveInfinity(specifiedValue) ? defaultValue : specifiedValue;

        protected override bool IsArrangeValid(Self oldElement) =>
            NaturalSize == oldElement.NaturalSize;

        protected override bool IsRenderValid(Self oldElement) =>
            oldElement.Fill == Fill &&
            oldElement.Stroke == Stroke &&
            oldElement.StrokeThickness == StrokeThickness &&
            oldElement.StrokeMiterLimit == StrokeMiterLimit &&
            oldElement.StrokeLineCap == StrokeLineCap &&
            oldElement.StrokeLineJoin == StrokeLineJoin;
    }
}
