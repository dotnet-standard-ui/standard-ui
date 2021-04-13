namespace Microsoft.StandardUI.Media
{
    public class RadialGradientBrush : GradientBrush
    {
        public Point Center { get; init; } = Point.CenterDefault;

        public Point GradientOrigin { get; init; } = Point.CenterDefault;

        public float RadiusX { get; init; } = 0.5f;
    }
}
