using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Interop;
using Microsoft.StandardUI.Wpf.Automation;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using Colors = System.Windows.Media.Colors;
using WpfRect = System.Windows.Rect;
using WpfSize = System.Windows.Size;
using DrawingContext = Microsoft.StandardUI.Drawing.DrawingContext;
using WpfDrawingContext = System.Windows.Media.DrawingContext;
using SkiaSharp;

namespace Microsoft.StandardUI.Wpf
{
    abstract class LayerBase : UIElement, ILayer
    {
        List<UIElement> children = new List<UIElement>();
        List<UIElement> renderedChildren = new List<UIElement>();
        protected WpfSize size;
        SKElement? element;
        SKPicture? renderCache;

        public LayerBase()
        {
            IsHitTestVisible = true;
            Focusable = false;
        }

        public Microsoft.StandardUI.Matrix TotalTransform { get; protected set; } = Microsoft.StandardUI.Matrix.Identity;

        public void AddChild(UIElement child)
        {
            children.Add(child);
            AddVisualChild(child);
        }

        ILayer ILayer.AddChild(ILayerNode child)
        {
            Layer layer;
            if (child is InputNode input)
                layer = new InputLayer(input);
            else if (child is NativeLayerNode native)
            {
                NativeLayer nativeLayer = new(this, native.Native);
                AddChild(native.Native);
                return nativeLayer;
            }
            else
                layer = new Layer(child);
            AddChild(layer);
            return layer;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            ParentLayer?.RemoveChild(this);
        }

        public void RemoveChild(UIElement child)
        {
            children.Remove(child);
            renderedChildren.Remove(child);
            RemoveVisualChild(child);
        }

        public void OnChildRendered(UIElement child) =>
            renderedChildren.Add(child);

        protected override Visual GetVisualChild(int index)
        {
            if (element != null)
            {
                if (index == 0)
                    return element;
                return children[index - 1];
            }

            return children[index];
        }

        protected override int VisualChildrenCount =>
            children.Count + (element != null ? 1 : 0);

        protected override void ArrangeCore(WpfRect finalRect)
        {
            base.ArrangeCore(finalRect);
            element?.Measure(size);
            element?.Arrange(new(size));
        }

        private void Element_PaintSurface(object? sender, SKPaintSurfaceEventArgs args)
        {
            if (renderCache == null)
                return;

            var canvas = args.Surface.Canvas;
            canvas.Clear();
            canvas.DrawPicture(renderCache, 0, 0);
            renderCache = null;
        }

        protected override void OnRender(WpfDrawingContext drawingContext)
        {
            var source = PresentationSource.FromVisual(this);
            var dpix = (float)source.CompositionTarget.TransformToDevice.M11;
            var dpiy = (float)source.CompositionTarget.TransformToDevice.M22;

            var skPictureRecorder = new SKPictureRecorder();

            // TODO: Can we restrict the culling rect here?
            SKRect skCullingRect = SKRect.Create(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
            SKCanvas canvas = skPictureRecorder.BeginRecording(skCullingRect);

            DrawingContext context = new DrawingContext(canvas);
            context.Push(Matrix.Scale(dpix, dpiy));
            RenderLayer(context);
            renderCache = skPictureRecorder.EndRecording();

            if (context.IsVisible)
            {
                if (element != null)
                    element.InvalidateVisual();
                else
                {
                    element = new();
                    element.PaintSurface += Element_PaintSurface;
                    AddVisualChild(element);
                    element.Measure(size);
                    element.Arrange(new(size));
                }
            }

            if (renderedChildren.Count > 0)
            {
                // In some cases children will be removed before measure is called again.
                // Don't update child order unless we're up to date.
                if (children.Count == renderedChildren.Count)
                    children = renderedChildren;
                renderedChildren = new List<UIElement>();
            }

            base.OnRender(drawingContext);
        }

        protected abstract void RenderLayer(DrawingContext context);

        protected override HitTestResult? HitTestCore(PointHitTestParameters hitTestParameters) =>
            new PointHitTestResult(this, hitTestParameters.HitPoint);

        public void OnRemove(object native)
        {
            if (native is UIElement element)
                RemoveChild(element);
        }

        public void OnArrange((Size, float?) result)
        {
            this.size = result.Item1.Into();
            InvalidateVisual();
        }

        void ILayer.OnRender(DrawingContext drawingContext)
        {
            var parentLayer = ParentLayer;
            parentLayer?.OnChildRendered(this);

            TotalTransform = drawingContext.TotalTransform;

            // TODO support rotation. Render transform somehow?
            // TODO validate correctness with respect to scale
            var parentTransform = parentLayer?.TotalTransform ?? Microsoft.StandardUI.Matrix.Identity;
            var parentOffsetX = parentTransform.m31;
            var parentOffsetY = parentTransform.m32;
            var offsetX = TotalTransform.m31 * (parentTransform.m11 / TotalTransform.m11);
            var offsetY = TotalTransform.m32 * (parentTransform.m22 / TotalTransform.m22);
            offsetX -= parentOffsetX;
            offsetY -= parentOffsetY;
            Arrange(new global::System.Windows.Rect(offsetX, offsetY, size.Width, size.Height));
        }

        public virtual void OnUpdated() { }

        public void InvalidateRender() => InvalidateVisual();

        public virtual void InvalidateLayout()
        {
            ParentLayer?.InvalidateArrange();
            InvalidateArrange();
        }

        void IFocus.Focus() => Focus();
        void IFocus.FocusNext() { }
        void IFocus.FocusPrevious() { }

        public void Reparent(ILayer newParent)
        {
            ParentLayer?.RemoveChild(this);
            ((LayerBase)newParent).AddChild(this);
        }

        LayerBase? ParentLayer => VisualTreeHelper.GetParent(this) as LayerBase;
    }
}
