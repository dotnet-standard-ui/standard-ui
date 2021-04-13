namespace Microsoft.StandardUI.Media
{
    public class ArcSegment : PathSegment
    {
        public Point Point { get; init; }

        public Size Size { get; init; }

        public float RotationAngle { get; init; } = 0;

        public bool IsLargeArc { get; init; } = false;

        public SweepDirection SweepDirection { get; init; } = SweepDirection.Counterclockwise;
    }
}
