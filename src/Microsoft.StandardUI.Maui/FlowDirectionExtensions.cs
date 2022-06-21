using System;

namespace Microsoft.StandardUI.Maui
{
    public static class FlowDirectionExtensions
    {
        public static Microsoft.Maui.FlowDirection ToMauiFlowDirection(this FlowDirection flowDirection) =>
            flowDirection switch
            {
                FlowDirection.LeftToRight => Microsoft.Maui.FlowDirection.LeftToRight,
                FlowDirection.RightToLeft => Microsoft.Maui.FlowDirection.RightToLeft,
                _ => throw new ArgumentOutOfRangeException(nameof(flowDirection), $"Invalid FlowDirection value: {flowDirection}"),
            };

        public static FlowDirection ToStandardUIFlowDirection(this Microsoft.Maui.FlowDirection flowDirection) =>
            flowDirection switch
            {
                Microsoft.Maui.FlowDirection.LeftToRight => FlowDirection.LeftToRight,
                Microsoft.Maui.FlowDirection.RightToLeft => FlowDirection.RightToLeft,
                Microsoft.Maui.FlowDirection.MatchParent => throw new NotSupportedException($"MAUI FlowDirection MatchParent isn't currently supported in StandardUI"),
                _ => throw new ArgumentOutOfRangeException(nameof(flowDirection), $"Invalid FlowDirection value: {flowDirection}"),
            };
    }
}
