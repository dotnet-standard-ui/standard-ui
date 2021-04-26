using Microsoft.StandardUI.Drawing;

namespace Microsoft.StandardUI.Shapes
{
    public class Ellipse : Shape<Ellipse>
    {
        /// <summary>
        /// The width of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        public float Width { get; init; } = float.NaN;

        /// <summary>
        /// The height of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        public float Height { get; init; } = float.NaN;

        public override Size NaturalSize => new Size(Width, Height);

        public override void Render(DrawingContext context, Size size) =>
            context.DrawEllipse(this, size);
    }
}
