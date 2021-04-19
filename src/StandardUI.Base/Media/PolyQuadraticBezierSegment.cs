using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PolyQuadraticBezierSegment : PathSegment
    {
        public Points Points { get; init; } = Points.Default;
    }
}
