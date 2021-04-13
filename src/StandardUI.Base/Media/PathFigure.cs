using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PathFigure
    {
        public IReadOnlyList<PathSegment> Segments { get; init; } = Array.Empty<PathSegment>();

        // TODO: Supply default point
        public Point StartPoint { get; init; }

        public bool IsClosed { get; init; } = false;

        public bool IsFilled { get; init; } = true;
    }
}
