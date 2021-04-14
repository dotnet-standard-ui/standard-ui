using System;
using System.Diagnostics;
using AppKit;
using CoreGraphics;
using Foundation;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Drawing;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Interop;
using Microsoft.StandardUI.Tree;
using SkiaSharp;

namespace Microsoft.StandardUI.Cocoa
{
    [Register("NSStandardUIHost")]
    public class NSStandardUIHost : NSView
    {
        FontManager fontManager = new(SKFontManager.Default);
        StateManager stateManager;
        Root root;
        RootLayer rootLayer;

        public NSStandardUIHost() => Initialize();
        public NSStandardUIHost(IntPtr handle) : base(handle) => Initialize();
        public NSStandardUIHost(CGRect frameRect) : base(frameRect) => Initialize();

        public static void Reference() { }

        void Initialize()
        {
            FocusRingType = NSFocusRingType.None;
            CanDrawConcurrently = false;

            stateManager = new(BeginInvokeOnMainThread);
            rootLayer = new();
            rootLayer.Frame = Bounds;
            AddSubview(rootLayer);
            root = new(CreateContext(), () =>
                new Column(
                    new TextBlock("Hello!!"),
                    new Row(
                        VerticalAlignment.Center,
                        new Native.NSButton()
                            .Title("NSButton")
                            .BezelStyle(NSBezelStyle.Rounded)
                            .Activated(() => Debug.WriteLine("Hello activation")),
                        new TextBlock("Trailing text")),
                    new TextBlock("zzz")));
            root.RootNodeChanged += Root_RootNodeChanged;
            rootLayer.Root = root.RootNode;
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                rootLayer.Frame = Bounds;
            }
        }

        public override void DrawRect(CGRect dirtyRect)
        {
            base.DrawRect(dirtyRect);
        }

        public override CGSize IntrinsicContentSize => rootLayer.IntrinsicContentSize;

        public Func<Element> RootElement
        {
            get => root.RootElement;
            set => root.RootElement = value;
        }

        public override void ViewDidMoveToWindow()
        {
            base.ViewDidMoveToWindow();
            root.Context = CreateContext();
        }

        Context CreateContext()
        {
            // TODO Update context if any of these properties change.
            // Not sure how to check for changes. Maybe recheck every render?
            var window = Window;
            var scale = (float)(window?.BackingScaleFactor ?? 1);
            var direction = UserInterfaceLayoutDirection;
            FlowDirection flowDirection;
            switch (direction)
            {
                case NSUserInterfaceLayoutDirection.RightToLeft:
                    flowDirection = FlowDirection.RightToLeft;
                    break;
                default:
                    flowDirection = FlowDirection.LeftToRight;
                    break;
            }

            // Font rendering isn't crisp... Might have scale wrong.
            var systemTypeface = fontManager.CreateTypeface("Helvetica Neue");
            TextTheme textTheme = new(systemTypeface, 12, Colors.Black);

            return new(stateManager, rootLayer, new()
            {
                { typeof(DpiScale), new DpiScale(scale, scale) },
                { typeof(IFontManager), fontManager },
                { typeof(TextTheme), textTheme },
                { typeof(FlowDirection), flowDirection }
            });
        }

        void Root_RootNodeChanged(object sender, EventArgs e) =>
            rootLayer.Root = root.RootNode;
    }
}
