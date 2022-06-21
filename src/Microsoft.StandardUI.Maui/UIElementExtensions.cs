using System;

namespace Microsoft.StandardUI.Maui
{
    public static class UIElementExtensions
    {
        public static Microsoft.Maui.Controls.View ToView(this IUIElement standardUIElement)
        {
            if (standardUIElement is Microsoft.Maui.Controls.View view)
                return view;

#if TODO
            if (standardUIElement is NativeUIElement nativeUIElement)
                return nativeUIElement.FrameworkElement;
#endif

            throw new InvalidOperationException($"UIElement is of unexpected type '{standardUIElement.GetType()}' and can't be converted to a native MAUI View");
        }
    }
}
