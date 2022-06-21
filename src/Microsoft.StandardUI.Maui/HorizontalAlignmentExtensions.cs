using System;

namespace Microsoft.StandardUI.Maui
{
    public static class HorizontalAlignmentExtensions
    {
        public static Microsoft.Maui.Controls.LayoutAlignment ToMauiLayoutAlignment(this HorizontalAlignment horizontalAligmnet) =>
            horizontalAligmnet switch
            {
                HorizontalAlignment.Left => Microsoft.Maui.Controls.LayoutAlignment.Start,
                HorizontalAlignment.Center => Microsoft.Maui.Controls.LayoutAlignment.Center,
                HorizontalAlignment.Right => Microsoft.Maui.Controls.LayoutAlignment.End,
                HorizontalAlignment.Stretch => Microsoft.Maui.Controls.LayoutAlignment.Fill,
                _ => throw new ArgumentOutOfRangeException(nameof(horizontalAligmnet), $"Invalid HorizontalAlignment value: {horizontalAligmnet}"),
            };

        public static HorizontalAlignment ToStandardUIHorizontalAlignment(this Microsoft.Maui.Controls.LayoutAlignment layoutAlignment) =>
            layoutAlignment switch
            {
                Microsoft.Maui.Controls.LayoutAlignment.Start => HorizontalAlignment.Left,
                Microsoft.Maui.Controls.LayoutAlignment.Center => HorizontalAlignment.Center,
                Microsoft.Maui.Controls.LayoutAlignment.End => HorizontalAlignment.Right,
                Microsoft.Maui.Controls.LayoutAlignment.Fill => HorizontalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(layoutAlignment), $"Invalid LayoutAlignment value: {layoutAlignment}"),
            };
    }
}
