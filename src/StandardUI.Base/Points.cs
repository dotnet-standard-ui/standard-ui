using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.StandardUI
{
    public struct Points
    {
        private readonly Point[] pointsArray;

        public static readonly Points Default = new Points(Array.Empty<Point>());

        public Points(Point[] points)  => pointsArray = points;

        public Points(List<Point> points) => pointsArray = points.ToArray(); 

        public int Length => pointsArray.Length;

        public Point this[int index] => pointsArray[index];
    }
}
