// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Controls
{
    public partial class Canvas : Panel, ICanvas
    {
        public static readonly BindableProperty LeftProperty = PropertyUtils.RegisterAttached("Left", typeof(double), typeof(VisualElement), 0.0);
        public static readonly BindableProperty TopProperty = PropertyUtils.RegisterAttached("Top", typeof(double), typeof(VisualElement), 0.0);
        
        public static double GetLeft(VisualElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(VisualElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(VisualElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(VisualElement element, double value) => element.SetValue(TopProperty, value);
    }
}
