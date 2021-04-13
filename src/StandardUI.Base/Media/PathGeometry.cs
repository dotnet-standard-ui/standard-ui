using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PathGeometry : Geometry
    {
        public IReadOnlyList<PathFigure> Figures { get; init; } = Array.Empty<PathFigure>();

        public FillRule FillRule { get; init; } = FillRule.EvenOdd;
    }
}
