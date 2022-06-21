// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        public static CanvasAttached Instance = new CanvasAttached();
        
        public double GetLeft(IUIElement element) => Canvas.GetLeft(element.ToView());
        public void SetLeft(IUIElement element, double value) => Canvas.SetLeft(element.ToView(), value);
        
        public double GetTop(IUIElement element) => Canvas.GetTop(element.ToView());
        public void SetTop(IUIElement element, double value) => Canvas.SetTop(element.ToView(), value);
    }
}
