using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Interop;
using SkiaSharp;
using SkiaSharp.Views.Mac;

namespace Microsoft.StandardUI.Cocoa
{
    abstract class LayerBase : NSView, ILayer
    {
        Size measuredSize;
        bool layoutValid = false;
        List<Action<SKCanvas>> render;
        List<NSView> renderOrder;

        public LayerBase()
        {
        }


        [Export("requiresConstraintBasedLayout")]
        public static new bool RequiresConstraintBasedLayout() => false;

        public LayerBase ParentLayer => Superview as LayerBase;
        public Matrix TotalTransform { get; private set; } = Matrix.Identity;

        public void OnChildRendered(NSView child) => renderOrder?.Add(child); 

        public override void DrawRect(CGRect dirtyRect)
        {
            var scaleFactor = Window.BackingScaleFactor;
            render = new();
            DrawingContext childContext = new(render);
            renderOrder = new();
            childContext.Push(Matrix.Scale((float)scaleFactor, (float)scaleFactor));
            Render(childContext);

            var viewToIndex = renderOrder
                .Select((view, idx) => (view, idx))
                .ToDictionary(item => item.Item1, item => item.Item2);
            var totalViews = viewToIndex.Count;
            SortSubviews((a, b) =>
            {
                if (!viewToIndex.TryGetValue(a, out var aidx))
                    aidx = totalViews;
                if (!viewToIndex.TryGetValue(b, out var bidx))
                    bidx = totalViews;
                var cmp = aidx - bidx;
                if (cmp < 0)
                    return NSComparisonResult.Ascending;
                else if (cmp > 0)
                    return NSComparisonResult.Descending;
                else
                    return NSComparisonResult.Same;
            });

            if (childContext.IsVisible)
            {
                if (!WantsLayer)
                {
                    SKGLLayer glLayer = new();
                    glLayer.ContentsScale = scaleFactor;
                    glLayer.Opaque = false;
                    glLayer.PaintSurface += Layer_PaintSurface;
                    Layer = glLayer;
                    WantsLayer = true;
                }

                Layer.SetNeedsDisplay();
            }
            else if (WantsLayer)
                Layer.SetNeedsDisplay();
        }

        protected abstract void Render(DrawingContext context);

        public ILayer AddChild(ILayerNode child)
        {
            if (child is INativeCocoaNode native)
            {
                AddSubview(native.View);
                return new NativeLayer(this);
            }
            else
            {
                Layer layer = new(child);
                AddSubview(layer);
                return layer;
            }
        }

        public void Focus()
        {
            throw new NotImplementedException();
        }

        public void FocusNext()
        {
            throw new NotImplementedException();
        }

        public void FocusPrevious()
        {
            throw new NotImplementedException();
        }

        public void InvalidateLayout()
        {
            if (!layoutValid)
                return;

            layoutValid = false;
            ParentLayer?.InvalidateLayout();
        }

        public void InvalidateRender()
        {
            NeedsDisplay = true;
        }

        public void OnArrange((Size, float?) size)
        {
            measuredSize = size.Item1;
            layoutValid = true;
        }

        public void OnRender(DrawingContext drawingContext)
        {
            ParentLayer?.OnChildRendered(this);

            // TODO handle rotation/skew/scale
            TotalTransform = drawingContext.TotalTransform;
            var parentTransform = ParentLayer?.TotalTransform ?? Matrix.Identity;

            var offsetX = TotalTransform.m31 - parentTransform.m31;
            var offsetY = TotalTransform.m32 - parentTransform.m32;
            Frame = new(offsetX, offsetY, measuredSize.Width, measuredSize.Height);
            NeedsToDraw(Bounds);
        }

        void Layer_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (render == null)
                return;

            var canvas = e.Surface.Canvas;
            canvas.Clear();
            foreach (var action in render)
                action(canvas);

            render = null;
        }

        public void OnUpdated() { }

        public void Reparent(ILayer newParent)
        {
            RemoveFromSuperview();
            ((LayerBase)newParent).AddSubview(this);
        }
    }
}
