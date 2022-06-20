// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Controls
{
    public class Panel : StandardUIView, IPanel
    {
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        private UIElementCollection _uiElementCollection;
        
        public Panel()
        {
            _uiElementCollection = new UIElementCollection(this);
            SetValue(ChildrenProperty, _uiElementCollection);
        }
        
        public UIElementCollection Children => _uiElementCollection;
        IUIElementCollection IPanel.Children => Children;
    }
}
