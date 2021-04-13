using System;
using System.Diagnostics;

namespace Microsoft.StandardUI
{
    [DebuggerDisplay("{X}, {Y}")]
    public struct Point : IEquatable<Point>
    {
        public static readonly Point CenterDefault = new Point(0.5f, 0.5f);

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public static bool operator ==(Point left, Point right) =>
            left.Equals(right);

        public static bool operator !=(Point left, Point right) =>
            !(left == right);

        public static Point operator +(Point left, Point right) =>
            new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator -(Point left, Point right) =>
            new Point(left.X - right.X, left.Y - right.Y);

        public bool Equals(Point other) =>
            X == other.X && Y == other.Y;

        public override bool Equals(object? obj) =>
            obj is Point point && Equals(point);

        public override int GetHashCode() =>
            X.GetHashCode() ^ Y.GetHashCode();
    }
}
