// This file is generated from IUIElement.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI
{
    public static class UIElementExtensions
    {
        public static T Width<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.Width = value;
            return uiElement;
        }
        
        public static T MinWidth<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.MinWidth = value;
            return uiElement;
        }
        
        public static T MaxWidth<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.MaxWidth = value;
            return uiElement;
        }
        
        public static T HorizontalAlignment<T>(this T uiElement, HorizontalAlignment value) where T : IUIElement
        {
            uiElement.HorizontalAlignment = value;
            return uiElement;
        }
        
        public static T Height<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.Height = value;
            return uiElement;
        }
        
        public static T MinHeight<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.MinHeight = value;
            return uiElement;
        }
        
        public static T MaxHeight<T>(this T uiElement, double value) where T : IUIElement
        {
            uiElement.MaxHeight = value;
            return uiElement;
        }
        
        public static T Margin<T>(this T uiElement, Thickness value) where T : IUIElement
        {
            uiElement.Margin = value;
            return uiElement;
        }
        
        public static T VerticalAlignment<T>(this T uiElement, VerticalAlignment value) where T : IUIElement
        {
            uiElement.VerticalAlignment = value;
            return uiElement;
        }
        
        public static T FlowDirection<T>(this T uiElement, FlowDirection value) where T : IUIElement
        {
            uiElement.FlowDirection = value;
            return uiElement;
        }
        
        public static T Visible<T>(this T uiElement, bool value) where T : IUIElement
        {
            uiElement.Visible = value;
            return uiElement;
        }
    }
}
