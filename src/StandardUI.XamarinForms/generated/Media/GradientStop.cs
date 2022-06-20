// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class GradientStop : StandardUIBindableObject, IGradientStop
    {
        public static readonly BindableProperty ColorProperty = PropertyUtils.Register(nameof(Color), typeof(ColorXamarinForms), typeof(GradientStop), ColorXamarinForms.Default);
        public static readonly BindableProperty OffsetProperty = PropertyUtils.Register(nameof(Offset), typeof(double), typeof(GradientStop), 0.0);
        
        public ColorXamarinForms Color
        {
            get => (ColorXamarinForms) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color IGradientStop.Color
        {
            get => Color.Color;
            set => Color = new ColorXamarinForms(value);
        }
        
        public double Offset
        {
            get => (double) GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
    }
}
