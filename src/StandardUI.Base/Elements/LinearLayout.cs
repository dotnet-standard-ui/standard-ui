using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.StandardUI.Elements
{
    class LinearLayout
    {
        public static LinearLayout Horizontal(DpiScale scale, VerticalAlignment defaultAlignemnt,
            FlowDirection flow) =>
            new(0, scale.X, scale, HorizontalAlignment.Left, defaultAlignemnt, flow);

        public static LinearLayout Vertical(DpiScale scale, HorizontalAlignment defaultAlignment,
            FlowDirection flow) =>
            new(1, scale.Y, scale, defaultAlignment, VerticalAlignment.Top, flow);

        LinearLayout(int mainAxis, float scale, DpiScale dpiScale,
            HorizontalAlignment defaultHorizontal, VerticalAlignment defaultVertical, FlowDirection flow)
        {
            MainAxis = mainAxis;
            Scale = scale;
            InvScale = 1 / Scale;
            DpiScale = dpiScale;
            DefaultVertical = defaultVertical;
            DefaultHorizontal = defaultHorizontal;
            Flow = flow;
        }

        int MainAxis { get; }
        int CrossAxis => (MainAxis + 1) % 2;
        float Scale { get; }
        float InvScale { get; }
        DpiScale DpiScale { get; }
        VerticalAlignment DefaultVertical { get; }
        HorizontalAlignment DefaultHorizontal { get; }
        FlowDirection Flow { get; }

        public (Size, float?) Arrange(Size availableSize, IEnumerable<(Node, Expand)> children)
        {
            float remaining = availableSize[MainAxis];
            float availableCross = availableSize[CrossAxis];
            int totalFlex = 0;
            int expandCount = 0;
            float baseline = 0;
            float afterBaseline = 0;
            float actualCross = 0;
            bool hasBasline = false;

            // Layout non-flex children allowing them to take up all available space.
            foreach (var (node, expand) in children)
            {
                if (expand.Factor != 0)
                {
                    totalFlex += expand.Factor;
                    ++expandCount;
                }
                else
                {
                    var childAvailable = MakeSize(remaining, availableCross);
                    var (childActual, childBaseline) = node.Arrange(childAvailable);
                    var alongMain = Ceil(childActual[MainAxis]);
                    remaining = Math.Max(remaining - alongMain, 0);

                    if (MainAxis == 0 &&
                        Aligned.GetVerticalAlignment(node.RawElement, DefaultVertical) == VerticalAlignment.Baseline)
                    {
                        // If elements are positioned with baseline alignment the height isn't
                        // enough to determine the final size. We need to take into account elements
                        // with a small baseline compared to their total height.
                        float resolvedChildBaseline = childBaseline ?? childActual.Height;
                        float childAfterBaseline = childActual.Height - resolvedChildBaseline;
                        baseline = Math.Max(baseline, resolvedChildBaseline);
                        afterBaseline = Math.Max(afterBaseline, childAfterBaseline);
                        actualCross = Math.Max(actualCross, baseline + afterBaseline);
                        hasBasline = true;
                    }
                    else
                        actualCross = Math.Max(actualCross, childActual[CrossAxis]);
                }
            }

            if (expandCount > 0)
            {
                // Layout flex children dividing up the remaining space
                float spacePerUnit = totalFlex != 0 ? remaining / totalFlex : 0;
                spacePerUnit = Floor(spacePerUnit);
                foreach (var (node, expand) in children)
                {
                    if (expand.Factor != 0)
                    {
                        float space = spacePerUnit * expand.Factor;
                        var childSize = MakeSize(space, actualCross);
                        node.Arrange(childSize);
                    }
                }
            }

            // Place children along the main axis
            float pos = 0;
            foreach (var (node, expand) in children)
            {
                var size = node.Size;
                var offset = MakePoint(pos, 0);
                node.Offset = offset;

                float remainingWidth, remainingHeight;
                if (MainAxis == 0)
                {
                    remainingWidth = 0;
                    remainingHeight = actualCross - size.Height;
                }
                else
                {
                    remainingWidth = actualCross - size.Width;
                    remainingHeight = 0;
                }

                Aligned.Align(node, DpiScale, Flow, baseline, remainingWidth, remainingHeight,
                    DefaultHorizontal, DefaultVertical);
                pos += Ceil(size[MainAxis]);
            }

            float? maybeBaseline = baseline;
            if (!hasBasline)
                maybeBaseline = children
                    .Select(child => child.Item1)
                    .Where(child => child.Baseline != null)
                    .Select(child => child.Baseline!.Value + child.Offset.Y)
                    .FirstOrDefault();

            var flexSize = MakeSize(pos, actualCross);
            return (flexSize, maybeBaseline);
        }

        Point MakePoint(float mainAxisValue, float crossAxisValue)
        {
            Point point = new();
            point[MainAxis] = mainAxisValue;
            point[CrossAxis] = crossAxisValue;
            return point;
        }

        Size MakeSize(float mainAxisValue, float crossAxisValue)
        {
            Size size = new();
            size[MainAxis] = mainAxisValue;
            size[CrossAxis] = crossAxisValue;
            return size;
        }

        float Ceil(float size) => (float)Math.Ceiling(size * Scale) * InvScale;
        float Floor(float size) => (float)Math.Floor(size * Scale) * InvScale;
    }
}
