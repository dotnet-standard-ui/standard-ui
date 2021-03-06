// This file is generated from IPolygon.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public static readonly DependencyProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(Polygon), FillRule.EvenOdd);
        public static readonly DependencyProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsWinUI), typeof(Polygon), PointsWinUI.Default);
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
        
        public PointsWinUI Points
        {
            get => (PointsWinUI) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolygon.Points
        {
            get => Points.Points;
            set => Points = new PointsWinUI(value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawPolygon(this);
    }
}
