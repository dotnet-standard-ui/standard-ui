using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Tree;
using DrawingContext = Microsoft.StandardUI.Drawing.DrawingContext;
using WpfDrawingContext = System.Windows.Media.DrawingContext;
using WpfSize = System.Windows.Size;
using WpfRect = System.Windows.Rect;

namespace Microsoft.StandardUI.Wpf
{
    class RootLayer : LayerBase
    {
        Node? rootNode;

        public RootLayer() { }

        public Node? RootNode
        {
            get => rootNode;
            set
            {
                if (rootNode != value)
                {
                    rootNode = value;
                    InvalidateLayout();
                    InvalidateRender();
                }
            }
        }

        public override void InvalidateLayout()
        {
            base.InvalidateLayout();
            InvalidateMeasure();
        }

        protected override WpfSize MeasureCore(WpfSize availableSize)
        {
            var dpiScale = RootNode?.Context.Get<DpiScale>() ?? new DpiScale();
            TotalTransform = Matrix.Scale(dpiScale.X, dpiScale.Y);

            var (desired, baseline) = rootNode!.Arrange(availableSize.Into());
            OnArrange((desired, baseline));
            return desired.Into();
        }

        protected override void RenderLayer(DrawingContext context)
        {
            RootNode?.Render(context);
        }
    }
}
