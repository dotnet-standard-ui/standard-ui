using System;

namespace Microsoft.StandardUI.Wpf
{
    public static class FlowDirectionExtensions
    {
        public static System.Windows.FlowDirection ToWpfFlowDirection(this FlowDirection flowDirection) =>
            flowDirection switch
            {
                FlowDirection.LeftToRight => System.Windows.FlowDirection.LeftToRight,
                FlowDirection.RightToLeft => System.Windows.FlowDirection.RightToLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(flowDirection), $"Invalid FlowDirection value: {flowDirection}"),
            };

        public static FlowDirection ToStandardUIFlowDirection(this System.Windows.FlowDirection flowDirection) =>
            flowDirection switch
            {
                System.Windows.FlowDirection.LeftToRight => FlowDirection.LeftToRight,
                System.Windows.FlowDirection.RightToLeft => FlowDirection.RightToLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(flowDirection), $"Invalid FlowDirection value: {flowDirection}"),
            };
    }
}
