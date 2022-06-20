// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        public static CanvasAttached Instance = new CanvasAttached();
        
        public double GetLeft(IUIElement element) => Canvas.GetLeft((VisualElement) element);
        public void SetLeft(IUIElement element, double value) => Canvas.SetLeft((VisualElement) element, value);
        
        public double GetTop(IUIElement element) => Canvas.GetTop((VisualElement) element);
        public void SetTop(IUIElement element, double value) => Canvas.SetTop((VisualElement) element, value);
    }
}
