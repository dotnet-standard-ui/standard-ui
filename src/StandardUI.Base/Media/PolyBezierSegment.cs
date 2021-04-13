using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PolyBezierSegment : PathSegment
    {
        public IReadOnlyList<Point> Points { get; init; } = Array.Empty<Point>();
    }
}
