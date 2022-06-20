using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public class StandardUIView : View, IUIElement
    {
        public double MinWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double MaxWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double MinHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double MaxHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Size DesiredSize => throw new NotImplementedException();

        public double ActualX => throw new NotImplementedException();

        public double ActualY => throw new NotImplementedException();

        public double ActualWidth => throw new NotImplementedException();

        public double ActualHeight => throw new NotImplementedException();

        double IUIElement.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        double IUIElement.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Arrange(Rect finalRect)
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        public object GetValue(IUIProperty property)
        {
            BindableProperty bindableProperty = ((UIProperty)property).BindableProperty;
            return GetValue(bindableProperty);
        }

        public void Measure(Size availableSize)
        {
            throw new NotImplementedException();
        }

        public object ReadLocalValue(IUIProperty property)
        {
            throw new NotImplementedException();
        }

        public void SetValue(IUIProperty property, object value)
        {
            BindableProperty bindableProperty = ((UIProperty)property).BindableProperty;
            SetValue(bindableProperty, value);
        }
    }
}
