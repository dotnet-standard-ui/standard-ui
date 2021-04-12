using System;
using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Interop;

namespace Microsoft.StandardUI.Cocoa
{
    class Layer : LayerBase
    {
        public Layer(ILayerNode node)
        {
            Node = node;
        }

        public ILayerNode Node { get; }

        protected override void Render(DrawingContext context) =>
            Node.RenderLayer(context);
    }
}
