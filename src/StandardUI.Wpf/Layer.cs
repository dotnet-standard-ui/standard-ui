using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Interop;

namespace Microsoft.StandardUI.Wpf
{
    class Layer : LayerBase
    {
        public Layer(ILayerNode node) => Node = node;

        public ILayerNode Node { get; }

        protected override void RenderLayer(DrawingContext context) => Node.RenderLayer(context);
    }
}