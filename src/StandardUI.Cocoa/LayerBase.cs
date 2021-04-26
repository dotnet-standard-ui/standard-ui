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
        List<NSView> renderOrder;
        Content content;

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

            var skPictureRecorder = new SKPictureRecorder();

            // TODO: Can we restrict the culling rect here?
            SKRect skCullingRect = SKRect.Create(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
            SKCanvas canvas = skPictureRecorder.BeginRecording(skCullingRect);

            DrawingContext childContext = new(canvas);
            renderOrder = new();
            childContext.Push(Matrix.Scale((float)scaleFactor, (float)scaleFactor));
            Render(childContext);
            SKPicture render = skPictureRecorder.EndRecording();

            var viewToIndex = renderOrder
                .Select((view, idx) => (view, idx))
                .ToDictionary(item => item.Item1, item => item.Item2);
            if (content != null)
                viewToIndex[content] = -1;
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
                if (content == null)
                {
                    content = new();
                    AddSubview(content, NSWindowOrderingMode.Below, null);
                }
            }

            if (content != null)
            {
                content.ContentsScale = scaleFactor;
                content.Render = render;
                content.Frame = Bounds;
                content.Layer.SetNeedsDisplay();
            }
        }

        public override void UpdateLayer()
        {
            throw new NotImplementedException();
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

        public virtual void InvalidateLayout()
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

        public void OnUpdated() { }

        public void Reparent(ILayer newParent)
        {
            RemoveFromSuperview();
            ((LayerBase)newParent).AddSubview(this);
        }
    }
}
