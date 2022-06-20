// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly BindableProperty PointProperty = PropertyUtils.Register(nameof(Point), typeof(PointXamarinForms), typeof(LineSegment), PointXamarinForms.Default);
        
        public PointXamarinForms Point
        {
            get => (PointXamarinForms) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point ILineSegment.Point
        {
            get => Point.Point;
            set => Point = new PointXamarinForms(value);
        }
    }
}
