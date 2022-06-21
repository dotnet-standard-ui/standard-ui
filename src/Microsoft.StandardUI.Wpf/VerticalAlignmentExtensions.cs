using System;

namespace Microsoft.StandardUI.Wpf
{
    public static class VerticalAlignmentExtensions
    {
        public static System.Windows.VerticalAlignment ToWpfVerticalAlignment(this VerticalAlignment verticalAlignment) =>
            verticalAlignment switch
            {
                VerticalAlignment.Top => System.Windows.VerticalAlignment.Top,
                VerticalAlignment.Center => System.Windows.VerticalAlignment.Center,
                VerticalAlignment.Bottom => System.Windows.VerticalAlignment.Bottom,
                VerticalAlignment.Stretch => System.Windows.VerticalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(verticalAlignment), $"Invalid VerticalAlignment value: {verticalAlignment}"),
            };

        public static VerticalAlignment ToStandardUIVerticalAlignment(this System.Windows.VerticalAlignment verticalAlignment) =>
            verticalAlignment switch
            {
                System.Windows.VerticalAlignment.Top => VerticalAlignment.Top,
                System.Windows.VerticalAlignment.Center => VerticalAlignment.Center,
                System.Windows.VerticalAlignment.Bottom => VerticalAlignment.Bottom,
                System.Windows.VerticalAlignment.Stretch => VerticalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(verticalAlignment), $"Invalid VerticalAlignment value: {verticalAlignment}"),
            };
    }
}
