// This file is generated from IPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.XamarinForms.Media;
using Microsoft.StandardUI.Shapes;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Shapes
{
    public class Path : Shape, IPath
    {
        public static readonly BindableProperty DataProperty = PropertyUtils.Register(nameof(Data), typeof(Geometry), typeof(Path), null);
        
        public Geometry Data
        {
            get => (Geometry) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        IGeometry IPath.Data
        {
            get => Data;
            set => Data = (Geometry) value;
        }
        
        public override void Draw(IDrawingContext drawingContext) => drawingContext.DrawPath(this);
    }
}
