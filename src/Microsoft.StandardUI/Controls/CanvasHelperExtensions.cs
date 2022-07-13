using static Microsoft.StandardUI.StandardUIStatics;

namespace Microsoft.StandardUI.Controls
{
    public static class CanvasHelperExtensions
    {
        public static T _<T>(this T canvas, params IUIElement[] children) where T : ICanvas =>
            canvas.Children(children);

        public static T Add<T>(this T canvas, double left, double top, IUIElement child) where T : ICanvas
        {
            canvas.Children.Add(child.CanvasLeft(left).CanvasTop(top));
            return canvas;
        }

        public static T CanvasPosition<T>(this T uiElement, double left, double top) where T : IUIElement =>
            uiElement.CanvasLeft(left).CanvasTop(top);
    }
}
