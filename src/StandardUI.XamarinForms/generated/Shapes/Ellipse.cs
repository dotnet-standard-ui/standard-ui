// This file is generated from IEllipse.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Shapes;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Shapes
{
    public class Ellipse : Shape, IEllipse
    {
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawEllipse(this);
    }
}
