// This file is generated from IBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class BezierSegment : PathSegment, IBezierSegment
    {
        public static readonly BindableProperty Point1Property = PropertyUtils.Register(nameof(Point1), typeof(PointMaui), typeof(BezierSegment), PointMaui.Default);
        public static readonly BindableProperty Point2Property = PropertyUtils.Register(nameof(Point2), typeof(PointMaui), typeof(BezierSegment), PointMaui.Default);
        public static readonly BindableProperty Point3Property = PropertyUtils.Register(nameof(Point3), typeof(PointMaui), typeof(BezierSegment), PointMaui.Default);
        
        public PointMaui Point1
        {
            get => (PointMaui) GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }
        Point IBezierSegment.Point1
        {
            get => Point1.Point;
            set => Point1 = new PointMaui(value);
        }
        
        public PointMaui Point2
        {
            get => (PointMaui) GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
        Point IBezierSegment.Point2
        {
            get => Point2.Point;
            set => Point2 = new PointMaui(value);
        }
        
        public PointMaui Point3
        {
            get => (PointMaui) GetValue(Point3Property);
            set => SetValue(Point3Property, value);
        }
        Point IBezierSegment.Point3
        {
            get => Point3.Point;
            set => Point3 = new PointMaui(value);
        }
    }
}
