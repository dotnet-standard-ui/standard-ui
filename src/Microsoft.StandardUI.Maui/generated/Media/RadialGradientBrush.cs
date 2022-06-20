// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly BindableProperty CenterProperty = PropertyUtils.Register(nameof(Center), typeof(PointXamarinForms), typeof(RadialGradientBrush), PointXamarinForms.CenterDefault);
        public static readonly BindableProperty GradientOriginProperty = PropertyUtils.Register(nameof(GradientOrigin), typeof(PointXamarinForms), typeof(RadialGradientBrush), PointXamarinForms.CenterDefault);
        public static readonly BindableProperty RadiusXProperty = PropertyUtils.Register(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5);
        
        public PointXamarinForms Center
        {
            get => (PointXamarinForms) GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        Point IRadialGradientBrush.Center
        {
            get => Center.Point;
            set => Center = new PointXamarinForms(value);
        }
        
        public PointXamarinForms GradientOrigin
        {
            get => (PointXamarinForms) GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }
        Point IRadialGradientBrush.GradientOrigin
        {
            get => GradientOrigin.Point;
            set => GradientOrigin = new PointXamarinForms(value);
        }
        
        public double RadiusX
        {
            get => (double) GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
    }
}
