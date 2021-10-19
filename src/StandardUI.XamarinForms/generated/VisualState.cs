// This file is generated from IVisualState.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public class VisualState : UIPropertyObject, IVisualState
    {
        public static readonly BindableProperty NameProperty = PropertyUtils.Create(nameof(Name), typeof(string), typeof(VisualState), "");
        public static readonly BindableProperty SettersProperty = PropertyUtils.Create(nameof(Setters), typeof(SetterCollection), typeof(VisualState), null);
        
        private SetterCollection _setterCollection;
        
        public VisualState()
        {
            _setterCollection = new SetterCollection();
            SetValue(SettersProperty, _setterCollection);
        }
        
        public string Name => (string) GetValue(NameProperty);
        
        public SetterCollection Setters => _setterCollection;
        ISetterCollection IVisualState.Setters => Setters;
    }
}