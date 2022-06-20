// This file is generated from IPolyLineSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class PolyLineSegment : PathSegment, IPolyLineSegment
    {
        public static readonly BindableProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsXamarinForms), typeof(PolyLineSegment), PointsXamarinForms.Default);
        
        public PointsXamarinForms Points
        {
            get => (PointsXamarinForms) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyLineSegment.Points
        {
            get => Points.Points;
            set => Points = new PointsXamarinForms(value);
        }
    }
}
