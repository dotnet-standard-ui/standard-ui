using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PolyQuadraticBezierSegment : PathSegment
    {
        public IReadOnlyList<Point> Points { get; init; } = Array.Empty<Point>();
    }
}
