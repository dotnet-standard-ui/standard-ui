using Microsoft.StandardUI.Drawing;

namespace Microsoft.StandardUI.Shapes
{
    public class Rectangle : Shape<Rectangle>
    {
        /// <summary>
        /// The width of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        public float Width { get; init; } = float.NaN;

        /// <summary>
        /// The height of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        public float Height { get; init;  } = float.NaN;

        /// <summary>
        /// Gets or sets the x-axis radius of the ellipse that is used to round the corners of the rectangle.
        /// </summary>
        public float RadiusX { get; init; } = 0;

        /// <summary>
        /// Gets or sets the y-axis radius of the ellipse that is used to round the corners of the rectangle.
        /// </summary>
        public float RadiusY { get; init; } = 0;

        public override Size NaturalSize => new Size(Width, Height);

        public override void Render(DrawingContext context, Size size) =>
            context.DrawRectangle(this);

        protected override bool IsRenderValid(Rectangle oldElement) =>
            base.IsRenderValid(oldElement) &&
            oldElement.RadiusX == RadiusX &&
            oldElement.RadiusY == RadiusY;
    }
}
