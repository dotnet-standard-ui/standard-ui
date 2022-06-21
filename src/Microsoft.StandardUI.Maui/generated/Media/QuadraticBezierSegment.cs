// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public static readonly BindableProperty Point1Property = PropertyUtils.Register(nameof(Point1), typeof(PointMaui), typeof(QuadraticBezierSegment), PointMaui.Default);
        public static readonly BindableProperty Point2Property = PropertyUtils.Register(nameof(Point2), typeof(PointMaui), typeof(QuadraticBezierSegment), PointMaui.Default);
        
        public PointMaui Point1
        {
            get => (PointMaui) GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }
        Point IQuadraticBezierSegment.Point1
        {
            get => Point1.Point;
            set => Point1 = new PointMaui(value);
        }
        
        public PointMaui Point2
        {
            get => (PointMaui) GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
        Point IQuadraticBezierSegment.Point2
        {
            get => Point2.Point;
            set => Point2 = new PointMaui(value);
        }
    }
}
