using HarfBuzzSharp;
using Microsoft.StandardUI.Elements;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
using System;

namespace Microsoft.StandardUI.Drawing
{
    public class FontManager : IFontManager
    {
        SKFontManager fontManager;
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

        public IFormattedText FormatText(ITypeface typeface, Brush brush, float fontPt, DpiScale scale, FlowDirection flow, string text)
        {
            var realFontPt = fontPt * scale.X;
            var paint = brush.Into();
            paint.Typeface = (SKTypeface)typeface.NativeObject;
            paint.TextSize = realFontPt;

            HarfBuzzSharp.Buffer buffer = new();
            buffer.Direction = flow == FlowDirection.LeftToRight ? Direction.LeftToRight : Direction.RightToLeft;
            buffer.AddUtf16(text);

            SKFont font = new(paint.Typeface, realFontPt);
            var metrics = font.Metrics;

            SKShaper shaper = new((SKTypeface)typeface.NativeObject);
            var result = shaper.Shape(buffer, 0, -metrics.Ascent, paint);

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

            var blob = builder.Build();
            return new FormattedText(blob, width / scale.X, metrics.Ascent / scale.X, metrics.Descent / scale.X, scale.X, text, paint);
        }

        class Typeface : ITypeface
        {
            SKTypeface typeface;
            public Typeface(SKTypeface typeface) => this.typeface = typeface;

            public string FamilyName => typeface.FamilyName;

            public FontSlant Slant => throw new System.NotImplementedException();

            public int Weight => throw new System.NotImplementedException();

            public object NativeObject => typeface;
        }
    }
}