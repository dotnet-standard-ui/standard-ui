using System.Drawing;

namespace Microsoft.StandardUI.Media
{
    public class GradientStop
    {
        // The default is Transparent
        public Color Color { get; init; }

        public float Offset { get; init; } = 0;
    }
}
