// This file is generated from ITranslateTransform.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Media
{
    public class TranslateTransform : Transform, ITranslateTransform
    {
        public static readonly BindableProperty XProperty = PropertyUtils.Register(nameof(X), typeof(double), typeof(TranslateTransform), 0.0);
        public static readonly BindableProperty YProperty = PropertyUtils.Register(nameof(Y), typeof(double), typeof(TranslateTransform), 0.0);
        
        public double X
        {
            get => (double) GetValue(XProperty);
            set => SetValue(XProperty, value);
        }
        
        public double Y
        {
            get => (double) GetValue(YProperty);
            set => SetValue(YProperty, value);
        }
    }
}
