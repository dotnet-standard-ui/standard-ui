// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public static readonly BindableProperty Point1Property = PropertyUtils.Register(nameof(Point1), typeof(PointXamarinForms), typeof(QuadraticBezierSegment), PointXamarinForms.Default);
        public static readonly BindableProperty Point2Property = PropertyUtils.Register(nameof(Point2), typeof(PointXamarinForms), typeof(QuadraticBezierSegment), PointXamarinForms.Default);
        
        public PointXamarinForms Point1
        {
            get => (PointXamarinForms) GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }
        Point IQuadraticBezierSegment.Point1
        {
            get => Point1.Point;
            set => Point1 = new PointXamarinForms(value);
        }
        
        public PointXamarinForms Point2
        {
            get => (PointXamarinForms) GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
        Point IQuadraticBezierSegment.Point2
        {
            get => Point2.Point;
            set => Point2 = new PointXamarinForms(value);
        }
    }
}
