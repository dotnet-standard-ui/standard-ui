﻿using System;

namespace Microsoft.StandardUI.Wpf
{
    public static class HorizontalAlignmentExtensions
    {
        public static System.Windows.HorizontalAlignment ToWpfHorizontalAlignment(this HorizontalAlignment horizontalAligmnet)
        {
            return horizontalAligmnet switch
            {
                HorizontalAlignment.Left => System.Windows.HorizontalAlignment.Left,
                HorizontalAlignment.Center => System.Windows.HorizontalAlignment.Center,
                HorizontalAlignment.Right => System.Windows.HorizontalAlignment.Right,
                HorizontalAlignment.Stretch => System.Windows.HorizontalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(horizontalAligmnet), $"Invalid HorizontalAlignment value: {horizontalAligmnet}"),
            };
        }

        public static HorizontalAlignment FromWpfHorizontalAlignment(System.Windows.HorizontalAlignment horizontalAlignment)
        {
            return horizontalAlignment switch
            {
                System.Windows.HorizontalAlignment.Left => HorizontalAlignment.Left,
                System.Windows.HorizontalAlignment.Center => HorizontalAlignment.Center,
                System.Windows.HorizontalAlignment.Right => HorizontalAlignment.Right,
                System.Windows.HorizontalAlignment.Stretch => HorizontalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException(nameof(horizontalAlignment), $"Invalid HorizontalAlignment value: {horizontalAlignment}"),
            };
        }
    }
}