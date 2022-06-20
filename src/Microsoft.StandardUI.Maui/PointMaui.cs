using System.ComponentModel;
using Microsoft.StandardUI.XamarinForms.Converters;

namespace Microsoft.StandardUI.XamarinForms
{
    [TypeConverter(typeof(PointTypeConverter))]
    public struct PointXamarinForms

    {
        public static readonly PointXamarinForms Default = new PointXamarinForms(Point.Default);
        public static readonly PointXamarinForms CenterDefault = new PointXamarinForms(Point.CenterDefault);


        public Point Point { get; }

        public PointXamarinForms(Point point)
        {
            Point = point;
        }
    }
}
