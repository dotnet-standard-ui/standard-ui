// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Register(nameof(StartPoint), typeof(PointXamarinForms), typeof(LinearGradientBrush), PointXamarinForms.Default);
        public static readonly BindableProperty EndPointProperty = PropertyUtils.Register(nameof(EndPoint), typeof(PointXamarinForms), typeof(LinearGradientBrush), PointXamarinForms.Default);
        
        public PointXamarinForms StartPoint
        {
            get => (PointXamarinForms) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point ILinearGradientBrush.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointXamarinForms(value);
        }
        
        public PointXamarinForms EndPoint
        {
            get => (PointXamarinForms) GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
        Point ILinearGradientBrush.EndPoint
        {
            get => EndPoint.Point;
            set => EndPoint = new PointXamarinForms(value);
        }
    }
}
