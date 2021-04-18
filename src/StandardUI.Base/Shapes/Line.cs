using Microsoft.StandardUI.Drawing;
using System;

namespace Microsoft.StandardUI.Shapes
{
    public class Line : Shape<Line>
    {
        public float X1 { get; init; } = 0;

        public float Y1 { get; init; } = 0;

        public float X2 { get; init; } = 0;

        public float Y2 { get; init; } = 0;

        public override void Render(DrawingContext context, Size size) =>
            context.DrawLine(this);
    }
}
