using System;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    /// <summary>
    /// This is the base for predefined dependency objects;
    /// </summary>
    public class StandardUIDependencyObject : BindableObject, IUIPropertyObject
    {
        public object GetValue(IUIProperty property)
        {
            BindableProperty bindableProperty = ((UIProperty)property).BindableProperty;
            return GetValue(bindableProperty);
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
