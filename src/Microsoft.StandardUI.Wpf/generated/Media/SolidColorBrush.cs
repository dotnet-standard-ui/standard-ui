// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly DependencyProperty ColorProperty = PropertyUtils.Register(nameof(Color), typeof(ColorWpf), typeof(SolidColorBrush), ColorWpf.Default);
        
        public ColorWpf Color
        {
            get => (ColorWpf) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color ISolidColorBrush.Color
        {
            get => Color.Color;
            set => Color = new ColorWpf(value);
        }
    }
}
