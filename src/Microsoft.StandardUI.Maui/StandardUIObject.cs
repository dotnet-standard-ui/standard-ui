using Microsoft.Maui.Controls;
using System;

namespace Microsoft.StandardUI.Maui
{
    /// <summary>
    /// This is the base for predefined non-view bindable objects
    /// </summary>
    public class StandardUIObject : BindableObject, IUIObject
    {
        object? IUIObject.GetValue(IUIProperty property) => GetValue(((UIProperty)property).BindableProperty);
        object? IUIObject.ReadLocalValue(IUIProperty property) => throw new NotImplementedException();
        void IUIObject.SetValue(IUIProperty property, object? value) => SetValue(((UIProperty)property).BindableProperty, value);
        void IUIObject.ClearValue(IUIProperty property) => ClearValue(((UIProperty)property).BindableProperty);
    }
}
