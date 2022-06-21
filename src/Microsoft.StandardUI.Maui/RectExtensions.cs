namespace Microsoft.StandardUI.Maui
{
    public static class RectExtensions
    {
        public static Microsoft.Maui.Graphics.Rect ToMauiRect(this Rect rect) => new Microsoft.Maui.Graphics.Rect(rect.X, rect.Y, rect.Width, rect.Height);

        public static Rect ToStandardUIRect(this Microsoft.Maui.Graphics.Rect rect) => new Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
