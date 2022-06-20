// This file is generated from IGeometry.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;
using Microsoft.StandardUI.XamarinForms.Converters;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class Geometry : StandardUIBindableObject, IGeometry
    {
        public static readonly BindableProperty StandardFlatteningToleranceProperty = PropertyUtils.Register(nameof(StandardFlatteningTolerance), typeof(double), typeof(Geometry), 0.25);
        public static readonly BindableProperty TransformProperty = PropertyUtils.Register(nameof(Transform), typeof(Transform), typeof(Geometry), null);
        
        public double StandardFlatteningTolerance
        {
            get => (double) GetValue(StandardFlatteningToleranceProperty);
            set => SetValue(StandardFlatteningToleranceProperty, value);
        }
        
        public Transform Transform
        {
            get => (Transform) GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }
        ITransform IGeometry.Transform
        {
            get => Transform;
            set => Transform = (Transform) value;
        }
    }
}
