namespace Microsoft.StandardUI.Maui
{
    public static class SizeExtensions
    {
        public static Microsoft.Maui.Graphics.Size ToMauiSize(this Size size) => new Microsoft.Maui.Graphics.Size(size.Width, size.Height);

        public static Size ToStandardUISize(this Microsoft.Maui.Graphics.Size size) => new Size(size.Width, size.Height);
    }
}
