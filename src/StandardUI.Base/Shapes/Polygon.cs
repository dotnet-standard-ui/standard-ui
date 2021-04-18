using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Media;
using System;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Shapes
{
    public class Polygon : Shape<Polygon>
    {
        public FillRule FillRule { get; init; } = FillRule.EvenOdd;

        public Points Points { get; init; } = Points.Default;

        public override void Render(DrawingContext context, Size size) =>
            context.DrawPolygon(this);

        protected override bool IsRenderValid(Polygon oldElement) =>
            base.IsRenderValid(oldElement) &&
            oldElement.FillRule == FillRule;
    }
}
