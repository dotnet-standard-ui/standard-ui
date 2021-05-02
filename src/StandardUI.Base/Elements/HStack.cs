using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Tree;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.StandardUI.Elements
{
    public class HStack : Container<Expand>
    {
        public HStack(VerticalAlignment alignment, FlowDirection? flowDirection, params Expand[] children) : base(children)
        {
            Alignment = alignment;
            FlowDirection = flowDirection;
        }
        public HStack(VerticalAlignment alignment, params Expand[] children) : this(alignment, null, children) { }

        public HStack(FlowDirection? flowDirection, params Expand[] children) : this(VerticalAlignment.Top, flowDirection, children) { }

        public HStack(params Expand[] children) : this(VerticalAlignment.Top, children)
        { }

        public VerticalAlignment Alignment { get; }
        public FlowDirection? FlowDirection { get; }

        public override (Size, float?) Arrange(Context context, Size availableSize, List<Node> children)
        {
            var direction = FlowDirection ?? context.Get<FlowDirection>();
            var zipped = children.Zip(Children, (a, b) => (a, b));
            if (direction == Elements.FlowDirection.RightToLeft)
                zipped = zipped.Reverse();

            var scale = context.Get<DpiScale>();
            var layout = LinearLayout.Horizontal(scale, Alignment, direction);
            return layout.Arrange(availableSize, zipped);
        }

        public override bool IsLayoutInvalid(Container<Expand> oldContainer)
        {
            var oldHStack = (HStack)oldContainer;
            if (Alignment != oldHStack.Alignment)
                return true;
            return Children.Zip(oldHStack.Children, (a, b) => (a, b)).Any(pair => pair.a.Factor != pair.b.Factor);
        }

        public override Element ToElement(Expand child) => child.Child;
    }
}
