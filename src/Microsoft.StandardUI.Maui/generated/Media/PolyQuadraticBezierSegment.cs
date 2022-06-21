// This file is generated from IPolyQuadraticBezierSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class PolyQuadraticBezierSegment : PathSegment, IPolyQuadraticBezierSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsMaui), typeof(PolyQuadraticBezierSegment), PointsMaui.Default);
        
        public PointsMaui Points
        {
            get => (PointsMaui) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyQuadraticBezierSegment.Points
        {
            get => Points.Points;
            set => Points = new PointsMaui(value);
        }
    }
}
