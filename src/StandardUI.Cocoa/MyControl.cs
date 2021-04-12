using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Microsoft.StandardUI.Cocoa
{
    /// <summary>
    /// Test user control. Delete once Cocoa stuff is figured out.
    /// </summary>
    [Register("NSMyControl")]
    public class NSMyControl : NSControl
    {
        static int nextId;
        int id = nextId++;

        public NSMyControl() => Initialize();
        public NSMyControl(IntPtr handle) : base(handle) => Initialize();
        public NSMyControl(CGRect frameRect) : base(frameRect) => Initialize();

        public static void Reference() { }

        void Initialize()
        {
            WantsLayer = true;
            LayerContentsRedrawPolicy = NSViewLayerContentsRedrawPolicy.OnSetNeedsDisplay;
        }

        public override void DrawRect(CGRect dirtyRect)
        {
            base.DrawRect(dirtyRect);

            NSString str = new($"Hello World {id}");
            NSStringAttributes attributes = new()
            {
                Font = NSFont.SystemFontOfSize(12),
                ForegroundColor = NSColor.Black
            };
            str.DrawAtPoint(new CGPoint(0, 0), attributes);
        }
    }
}
