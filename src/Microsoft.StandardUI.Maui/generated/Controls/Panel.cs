// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.Maui.Controls;
using Microsoft.StandardUI.Controls;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Controls
{
    [ContentProperty("Children")]
    public class Panel : BuiltInUIElement, IPanel
    {
        public static readonly BindableProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection<Microsoft.Maui.Controls.View,Microsoft.StandardUI.IUIElement>), typeof(Panel), null);
        
        private UIElementCollection<Microsoft.Maui.Controls.View,Microsoft.StandardUI.IUIElement> _children;
        
        public Panel()
        {
            _children = new UIElementCollection<Microsoft.Maui.Controls.View,Microsoft.StandardUI.IUIElement>(this);
            SetValue(ChildrenProperty, _children);
        }
        
        public UIElementCollection<Microsoft.Maui.Controls.View,Microsoft.StandardUI.IUIElement> Children => _children;
        IUICollection<IUIElement> IPanel.Children => Children.ToStandardUIElementCollection();
    }
}
