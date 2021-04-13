using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;
using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Interop;
using Microsoft.StandardUI.Tree;

namespace Microsoft.StandardUI.Cocoa
{
    public class NativeCocoa<TNative> : Element where TNative : NSView
    {
        /// <summary>
        /// Adapt an arbitrary NSView to StandardUI
        /// </summary>
        /// <remarks>Size is computed from the view's intrinsic size</remarks>
        /// <param name="create">Creates a new instance of the underlying control. Only called once.</param>
        /// <param name="update">Updates the previously created instance with the current state.</param>
        public NativeCocoa(Func<TNative> create, Action<TNative> update) : this(create, update, IntrinsicSize)
        { }

        /// <summary>
        /// Adapt an arbitrary NSView to StandardUI
        /// </summary>
        /// <param name="create">Creates a new instance of the underlying control. Only called once.</param>
        /// <param name="update">Updates the previously created instance with the current state.</param>
        /// <param name="computeSize">Compute the desired frame size given the and available size.</param>
        public NativeCocoa(Func<TNative> create, Action<TNative> update, Func<TNative, Size, CGSize> computeSize)
        {
            Create = create;
            Update = update;
            ComputeSize = computeSize;
        }

        public Func<TNative> Create { get; }
        public Action<TNative> Update { get; }
        public Func<TNative, Size, CGSize> ComputeSize { get; }

        public override Node CreateNode(Node parent, Context context) =>
            new NativeCocoaNode<TNative>(parent, context, this);

        static CGSize IntrinsicSize(TNative view, Size incomming)
        {
            var insets = view.AlignmentRectInsets;
            var cgSize = view.IntrinsicContentSize;
            cgSize.Width += insets.Left + insets.Right;
            cgSize.Height += insets.Top + insets.Bottom;
            return cgSize;
        }
    }

    interface INativeCocoaNode
    {
        NSView View { get; }
    }

    class NativeCocoaNode<TNative> : NodeBase<NativeCocoa<TNative>>, ILayerNode, INativeCocoaNode where TNative : NSView
    {
        TNative view;
        NativeLayer layer;

        public NativeCocoaNode(Node parent, Context context, NativeCocoa<TNative> element) : base(parent, context, element)
        {
            view = element.Create();
            layer = (NativeLayer)Context.Layer.AddChild(this);
            Element.Update(view);
        }

        public NSView View => view;

        public override IEnumerable<Node> Children => Enumerable.Empty<Node>();

        public void RenderLayer(DrawingContext context) => throw new NotImplementedException();

        protected override void RenderOverride(DrawingContext context)
        {
            ((LayerBase)view.Superview).OnChildRendered(view);

            // TODO scale/skew/rotation
            var parentTransform = layer.Parent.TotalTransform;
            var transform = context.TotalTransform;
            var x = transform.m31 - parentTransform.m31;
            var y = transform.m32 - parentTransform.m32;

            var frame = view.Frame;
            frame.Location = new(0, view.Superview.Frame.Height - frame.Height - y);
            view.Frame = frame;
        }

        protected override (Size, float?) ArrangeOverride(Size availableSize)
        {
            var cgSize = Element.ComputeSize(view, availableSize);
            view.SetFrameSize(cgSize);

            // It's not clear that cocoa baseline is the exact equivalent of StandardUI baseline.
            var size = view.Frame.Size.Into();
            var baseline = view.BaselineOffsetFromBottom;
            if (baseline == 0)
                return (size, null);
            else
                return (size, (float)(cgSize.Height - baseline));
        }

        protected override void UpdateElement(NativeCocoa<TNative> oldElement, Context oldContext) { }
    }

    class NativeLayer : ILayer
    {
        public NativeLayer(LayerBase parent) => Parent = parent;

        public LayerBase Parent { get; private set; }

        public ILayer AddChild(ILayerNode child)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void InvalidateRender()
        {
            throw new NotImplementedException();
        }

        public void OnArrange((Size, float?) size)
        {
            throw new NotImplementedException();
        }

        public void OnRender(DrawingContext drawingContext)
        {
            throw new NotImplementedException();
        }

        public void OnUpdated()
        {
            throw new NotImplementedException();
        }

        public void Reparent(ILayer newParent)
        {
            throw new NotImplementedException();
        }
    }
}
