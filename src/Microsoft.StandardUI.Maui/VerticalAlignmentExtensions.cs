using System;

namespace Microsoft.StandardUI.Maui
{
    public static class VerticalAlignmentExtensions
    {
        public static Microsoft.Maui.Controls.LayoutAlignment ToMauiLayoutAlignment(this VerticalAlignment verticalAlignment) =>
            verticalAlignment switch
            {
                VerticalAlignment.Top => Microsoft.Maui.Controls.LayoutAlignment.Start,
                VerticalAlignment.Center => Microsoft.Maui.Controls.LayoutAlignment.Center,
                VerticalAlignment.Bottom => Microsoft.Maui.Controls.LayoutAlignment.End,
                VerticalAlignment.Stretch => Microsoft.Maui.Controls.LayoutAlignment.Fill,
                _ => throw new ArgumentOutOfRangeException(nameof(verticalAlignment), $"Invalid VerticalAlignment value: {verticalAlignment}"),
            };

        public static VerticalAlignment ToStandardUIVerticalAlignment(this Microsoft.Maui.Controls.LayoutAlignment layoutAlignment) =>
            layoutAlignment switch
            {
                Microsoft.Maui.Controls.LayoutAlignment.Start => VerticalAlignment.Top,
                Microsoft.Maui.Controls.LayoutAlignment.Center => VerticalAlignment.Center,
                Microsoft.Maui.Controls.LayoutAlignment.End => VerticalAlignment.Bottom,
                Microsoft.Maui.Controls.LayoutAlignment.Fill => VerticalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(layoutAlignment), $"Invalid LayoutAlignment value: {layoutAlignment}"),
            };
    }
}
