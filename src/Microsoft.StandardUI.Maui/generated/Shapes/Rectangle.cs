// This file is generated from IRectangle.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Shapes;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Shapes
{
    public class Rectangle : Shape, IRectangle
    {
        public static readonly BindableProperty RadiusXProperty = PropertyUtils.Register(nameof(RadiusX), typeof(double), typeof(Rectangle), 0.0);
        public static readonly BindableProperty RadiusYProperty = PropertyUtils.Register(nameof(RadiusY), typeof(double), typeof(Rectangle), 0.0);
        
        public double RadiusX
        {
            get => (double) GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
        
        public double RadiusY
        {
            get => (double) GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawRectangle(this);
    }
}
