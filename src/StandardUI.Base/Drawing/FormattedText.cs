using SkiaSharp;

namespace Microsoft.StandardUI.Drawing
{
    record FormattedText(SKTextBlob Blob, float Width, float Ascent, float Descent, float Scale, string Text, SKPaint Paint) : IFormattedText
    {
        public float Height => Descent - Ascent;
        public object NativeObject => Blob;
    }
}