namespace Microsoft.StandardUI.WinUI
{
    public static class ThicknessExtensions
    {
        public static Microsoft.UI.Xaml.Thickness ToWinUIThickness(this Thickness thickness) => new Microsoft.UI.Xaml.Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);

        public static Thickness ToStandardUIThickness(this Microsoft.UI.Xaml.Thickness thickness) => new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }
}
