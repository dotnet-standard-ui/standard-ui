namespace Microsoft.StandardUI.Shapes
{
    public class Rectangle : Shape
    {
        /// <summary>
        /// Gets or sets the x-axis radius of the ellipse that is used to round the corners of the rectangle.
        /// </summary>
        public float RadiusX { get; init; } = 0;

        /// <summary>
        /// Gets or sets the y-axis radius of the ellipse that is used to round the corners of the rectangle.
        /// </summary>
        public float RadiusY { get; init; } = 0;
    }
}
