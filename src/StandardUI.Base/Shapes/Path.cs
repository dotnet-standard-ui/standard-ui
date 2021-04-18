using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Shapes
{
    public class Path : Shape<Path>
    {
        public Geometry? Data { get; init; } = null;

        public override void Render(DrawingContext context, Size size)  => context.DrawPath(this);

        protected override bool IsArrangeValid(Path oldElement) =>
            Data == oldElement.Data;
    }
}
