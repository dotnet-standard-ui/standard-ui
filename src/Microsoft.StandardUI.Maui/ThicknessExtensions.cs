namespace Microsoft.StandardUI.Maui
{
    public static class ThicknessExtensions
    {
        public static Microsoft.Maui.Thickness ToMauiThickness(this Thickness thickness) => new Microsoft.Maui.Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);

        public static Thickness ToStandardUIThickness(this Microsoft.Maui.Thickness thickness) => new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }
}
