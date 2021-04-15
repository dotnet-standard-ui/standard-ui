using System;
using CoreGraphics;
using Microsoft.StandardUI.Tree;

namespace Microsoft.StandardUI.Cocoa
{
    class RootLayer : LayerBase
    {
        Size? intrinsicSize;

        public RootLayer()
        {
            NeedsLayout = true;
        }

        public Node Root { get; set; }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                if (!intrinsicSize.HasValue)
                {
                    (intrinsicSize, _) = Root.Arrange(new());
                }
                return intrinsicSize.Value.Into();
            }
        }

        protected override void Render(Drawing.DrawingContext context) =>
            Root?.Render(context);

        // TODO Integrate baseline


        public override void Layout()
        {
            Root.Arrange(Bounds.Size.Into());
            base.Layout();
        }

        public override void InvalidateLayout()
        {
            base.InvalidateLayout();
            NeedsLayout = true;
            InvalidateIntrinsicContentSize();
            Superview.InvalidateIntrinsicContentSize();
            InvalidateRender();
        }
    }
}
