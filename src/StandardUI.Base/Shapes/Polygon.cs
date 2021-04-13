using Microsoft.StandardUI.Media;
using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Shapes
{
    public class Polygon : Shape
    {
        public FillRule FillRule { get; init; } = FillRule.EvenOdd;

        public IReadOnlyList<Point> Points { get; init; } = Array.Empty<Point>();
    }
}
