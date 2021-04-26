using System;
using System.Collections.Generic;
using AppKit;
using SkiaSharp;
using SkiaSharp.Views.Mac;

namespace Microsoft.StandardUI.Cocoa
{
    /// <summary>
    /// Display the Skia based content of a LayerBase
    /// </summary>
    /// <remarks>
    /// Since Content contains an OpenGL view, it's only created when needed.
    /// Ideally we'd use an SKGLView, but I haven't figured out how to make it stop being opaque.
    /// </remarks>
    class Content : NSView
    {
        public Content()
        {
            SKGLLayer layer = new();
            layer.PaintSurface += Layer_PaintSurface;
            layer.Opaque = false;
            Layer = layer;
            WantsLayer = true;
        }

        public nfloat ContentsScale
        {
            get => Layer.ContentsScale;
            set => Layer.ContentsScale = value;
        }

        public SKPicture Render { get; set; }

        void Layer_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (Render == null)
                return;

            var canvas = e.Surface.Canvas;
            canvas.Clear();
            canvas.DrawPicture(Render, 0, 0);

            Render = null;
        }
    }
}
