using HarfBuzzSharp;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;

namespace Microsoft.StandardUI.Drawing
{
    public class FontManager : IFontManager
    {
        private SKFontManager fontManager;
        public FontManager(SKFontManager fontManager) => this.fontManager = fontManager;

        public ITypeface CreateTypeface(string fontName)
        {
            var skTypeface = fontManager.MatchFamily(fontName);
            return new Typeface(skTypeface);
        }

        public ITypeface CreateTypeface(string fontName, FontSlant slant, int weight)
        {
            var skTypeface = fontManager.MatchFamily(fontName, new SKFontStyle((SKFontStyleWeight)weight, SKFontStyleWidth.Normal, (SKFontStyleSlant)slant));
            return new Typeface(skTypeface);
        }

        public IFormattedText FormatText(ITypeface typeface, Brush brush, float fontPt, DpiScale scale, string text)
        {
            var realFontPt = fontPt * scale.X;
            var paint = brush.Into();
            paint.Typeface = (SKTypeface)typeface.NativeObject;
            paint.TextSize = realFontPt;

            HarfBuzzSharp.Buffer buffer = new();
            buffer.Direction = Direction.LeftToRight;
            buffer.AddUtf16(text);

            SKShaper shaper = new((SKTypeface)typeface.NativeObject);
            var result = shaper.Shape(buffer, paint);

            SKFont font = new(paint.Typeface, realFontPt);
            font.BaselineSnap = true;

            using SKTextBlobBuilder builder = new();
            var run = builder.AllocatePositionedRun(font, result.Codepoints.Length);
            var glyphs = run.GetGlyphSpan();
            var positions = run.GetPositionSpan();
            for (var i = 0; i < result.Codepoints.Length; ++i)
            {
                glyphs[i] = (ushort)result.Codepoints[i];
                positions[i] = result.Points[i];
            }

            float width = 0;
            if (glyphs.Length > 0)
            {
                var last = glyphs.Slice(glyphs.Length - 1);
                Span<float> widths = new(new float[1]);
                Span<SKRect> bounds = new(new SKRect[1]);
                font.GetGlyphWidths(last, widths, bounds, paint);
                width = result.Points[result.Points.Length - 1].X + widths[0];
            }

            var metrics = font.Metrics;
            var blob = builder.Build();
            return new FormattedText(blob, width / scale.X, metrics.Ascent / scale.Y, metrics.Descent / scale.Y, text, brush.Into());
        }

        private class Typeface : ITypeface
        {
            private SKTypeface typeface;
            public Typeface(SKTypeface typeface) => this.typeface = typeface;

            public string FamilyName => typeface.FamilyName;

            public FontSlant Slant => throw new System.NotImplementedException();

            public int Weight => throw new System.NotImplementedException();

            public object NativeObject => typeface;
        }
    }
}