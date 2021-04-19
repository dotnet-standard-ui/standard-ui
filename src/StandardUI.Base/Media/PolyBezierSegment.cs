using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Media
{
    public class PolyBezierSegment : PathSegment
    {
        public Points Points { get; init; } = Points.Default;
    }
}
