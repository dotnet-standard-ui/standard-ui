// This file is generated from IPolyline.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(Polyline), FillRule.EvenOdd);
        public static readonly BindableProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsXamarinForms), typeof(Polyline), PointsXamarinForms.Default);
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
        
        public PointsXamarinForms Points
        {
            get => (PointsXamarinForms) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyline.Points
        {
            get => Points.Points;
            set => Points = new PointsXamarinForms(value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawPolyline(this);
    }
}
