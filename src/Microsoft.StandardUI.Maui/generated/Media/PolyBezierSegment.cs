// This file is generated from IPolyBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class PolyBezierSegment : PathSegment, IPolyBezierSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsXamarinForms), typeof(PolyBezierSegment), PointsXamarinForms.Default);
        
        public PointsXamarinForms Points
        {
            get => (PointsXamarinForms) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyBezierSegment.Points
        {
            get => Points.Points;
            set => Points = new PointsXamarinForms(value);
        }
    }
}
