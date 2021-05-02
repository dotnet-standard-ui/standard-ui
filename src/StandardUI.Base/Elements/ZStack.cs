using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Interop;
using Microsoft.StandardUI.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.StandardUI.Elements
{
    public class ZStack : Container<Expand>
    {
        public ZStack(params Expand[] children) : base(children) { }

        public override (Size, float?) Arrange(Context context, Size availableSize, List<Node> children) =>
            throw new NotImplementedException();

        public override bool IsLayoutInvalid(Container<Expand> oldContainer) => false;

        public override Element ToElement(Expand child) => child.Child;

        public override Node CreateNode(Node? parent, Context context) =>
            new ZStackNode(parent, context, this);
    }

    internal class ZStackNode : NodeBase<ZStack>
    {
        private List<PseudoLayer> children;

        public ZStackNode(Node? parent, Context context, ZStack stack) : base(parent, context, stack)
        {
            var children = stack.Children;
            this.children = new List<PseudoLayer>(children.Count);

            PseudoLayer? previous = null;
            foreach (var expand in children)
            {
                var child = new PseudoLayer(this, context, expand.Child, previous);
                previous = child;
                this.children.Add(child);
            }
        }

        public override IEnumerable<Node> Children =>
            children.Select(child => child.Node);

        protected override (Size, float?) ArrangeOverride(Size availableSize)
        {
            // Consider adding alignment
            Size size = new();
            for (int i = 0; i < children.Count; ++i)
            {
                if (Element.Children[i].Factor == 0)
                {
                    var child = children[i];
                    var (childSize, childBaseline) = child.Arrange(availableSize);
                    size.Width = Math.Max(size.Width, childSize.Width);
                    size.Height = Math.Max(size.Height, childSize.Height);
                }
            }

            float? baseline = null;
            for (int i = 0; i < children.Count; ++i)
            {
                var expand = Element.Children[i];
                var child = children[i];
                if (expand.Factor != 0)
                    child.Arrange(size);

                if (child.Node.Baseline is float childBaseline)
                {
                    baseline = Math.Max(baseline ?? childBaseline, childBaseline);
                }
            }

            var dpi = Context.Get<DpiScale>();
            var flow = Context.Get<FlowDirection>();
            foreach (var child in Children)
            {
                var childSize = child.Size;
                var remainingWidth = size.Width - childSize.Width;
                var remainingHeight = size.Height - childSize.Height;
                Aligned.Align(child, dpi, flow, baseline ?? size.Height, remainingWidth, remainingHeight,
                    HorizontalAlignment.Left, VerticalAlignment.Top);

            }

            return (size, baseline);
        }

        public override void Dispose()
        {
            foreach (var layer in children)
                layer.Dispose();
        }

        protected override void RenderOverride(DrawingContext context)
        {
            foreach (var child in children)
                child.OnRender(context);
        }

        protected override void UpdateElement(ZStack oldStack, Context oldContext)
        {
            var newCount = Element.Children.Count;
            if (children.Count < newCount)
            {
                Context.InvalidateLayout();
                var previous = children.LastOrDefault();
                for (int i = children.Count; i < newCount; ++i)
                {
                    var child = new PseudoLayer(this, Context, Element.Children[i].Child, previous);
                    children.Add(child);
                    previous = child;
                }
            }
            else if (children.Count > newCount)
            {
                Context.InvalidateLayout();
                for (int i = newCount; i < children.Count; ++i)
                    children[i].Dispose();
                children.RemoveRange(newCount, children.Count - newCount);
            }

            for (int i = 0; i < newCount; ++i)
                children[i].UpdateElement(Element.Children[i].Child, Context);

        }
    }
}
