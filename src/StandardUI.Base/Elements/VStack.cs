using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Tree;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.StandardUI.Elements
{
    public class VStack : Container<Expand>
    {
        public VStack(HorizontalAlignment alignment, params Expand[] children) : base(children) =>
            Alignment = alignment;
        public VStack(params Expand[] children) : this(HorizontalAlignment.Left, children) { }

        public HorizontalAlignment Alignment { get; }

        public override (Size, float?) Arrange(Context context, Size availableSize, List<Node> children)
        {
            var scale = context.Get<DpiScale>();
            var flow = context.Get<FlowDirection>();
            var layout = LinearLayout.Vertical(scale, Alignment, flow);
            return layout.Arrange(availableSize, children.Zip(Children, (a, b) => (a, b)));
        }

        public override bool IsLayoutInvalid(Container<Expand> oldContainer)
        {
            var oldVStack = (VStack)oldContainer;
            if (oldVStack.Alignment != Alignment)
                return true;
            return Children.Zip(oldVStack.Children, (a, b) => (a, b)).Any(pair => pair.a.Factor != pair.b.Factor);
        }

        public override Element ToElement(Expand child) => child.Child;
    }
}
