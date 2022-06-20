// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly BindableProperty ColorProperty = PropertyUtils.Register(nameof(Color), typeof(ColorXamarinForms), typeof(SolidColorBrush), ColorXamarinForms.Default);
        
        public ColorXamarinForms Color
        {
            get => (ColorXamarinForms) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color ISolidColorBrush.Color
        {
            get => Color.Color;
            set => Color = new ColorXamarinForms(value);
        }
    }
}
