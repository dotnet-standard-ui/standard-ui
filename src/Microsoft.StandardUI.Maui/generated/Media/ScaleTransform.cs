// This file is generated from IScaleTransform.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class ScaleTransform : Transform, IScaleTransform
    {
        public static readonly BindableProperty CenterXProperty = PropertyUtils.Register(nameof(CenterX), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly BindableProperty CenterYProperty = PropertyUtils.Register(nameof(CenterY), typeof(double), typeof(ScaleTransform), 0.0);
        public static readonly BindableProperty ScaleXProperty = PropertyUtils.Register(nameof(ScaleX), typeof(double), typeof(ScaleTransform), 1.0);
        public static readonly BindableProperty ScaleYProperty = PropertyUtils.Register(nameof(ScaleY), typeof(double), typeof(ScaleTransform), 1.0);
        
        public double CenterX
        {
            get => (double) GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }
        
        public double CenterY
        {
            get => (double) GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }
        
        public double ScaleX
        {
            get => (double) GetValue(ScaleXProperty);
            set => SetValue(ScaleXProperty, value);
        }
        
        public double ScaleY
        {
            get => (double) GetValue(ScaleYProperty);
            set => SetValue(ScaleYProperty, value);
        }
    }
}
