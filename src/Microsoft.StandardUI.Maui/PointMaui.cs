using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.StandardUI.Maui.Converters;

namespace Microsoft.StandardUI.Maui
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct PointMaui
    {
        public static readonly PointMaui Default = new PointMaui(Point.Default);
        public static readonly PointMaui CenterDefault = new PointMaui(Point.CenterDefault);

        public Point Point { get; }

        public PointMaui(Point point)
        {
            Label label;

            Point = point;
        }
    }
}
