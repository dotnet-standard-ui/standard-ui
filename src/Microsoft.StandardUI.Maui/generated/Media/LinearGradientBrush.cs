// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Register(nameof(StartPoint), typeof(PointMaui), typeof(LinearGradientBrush), PointMaui.Default);
        public static readonly BindableProperty EndPointProperty = PropertyUtils.Register(nameof(EndPoint), typeof(PointMaui), typeof(LinearGradientBrush), PointMaui.Default);
        
        public PointMaui StartPoint
        {
            get => (PointMaui) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point ILinearGradientBrush.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointMaui(value);
        }
        
        public PointMaui EndPoint
        {
            get => (PointMaui) GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
        Point ILinearGradientBrush.EndPoint
        {
            get => EndPoint.Point;
            set => EndPoint = new PointMaui(value);
        }
    }
}
