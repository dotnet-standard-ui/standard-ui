// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly BindableProperty LeftProperty = PropertyUtils.RegisterAttached("Left", typeof(double), typeof(Microsoft.Maui.Controls.View), 0.0);
        public static readonly BindableProperty TopProperty = PropertyUtils.RegisterAttached("Top", typeof(double), typeof(Microsoft.Maui.Controls.View), 0.0);
        
        public static double GetLeft(Microsoft.Maui.Controls.View element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(Microsoft.Maui.Controls.View element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(Microsoft.Maui.Controls.View element) => (double) element.GetValue(TopProperty);
        public static void SetTop(Microsoft.Maui.Controls.View element, double value) => element.SetValue(TopProperty, value);
    }
}
