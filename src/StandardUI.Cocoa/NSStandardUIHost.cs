using System;
using System.Collections.Generic;
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
        static Dictionary<Type, object> globalContext = new();
        static event EventHandler globalContextChanged;

        FontManager fontManager = new(SKFontManager.Default);
        StateManager stateManager;
        Root root;
        RootLayer rootLayer;

        public NSStandardUIHost() => Initialize();
        public NSStandardUIHost(IntPtr handle) : base(handle) => Initialize();
        public NSStandardUIHost(CGRect frameRect) : base(frameRect) => Initialize();

        /// <summary>
        /// Sets additional context that will be added to every NSStandardUIHost
        /// </summary>
        /// <remarks>Do not modify the context dictionary once it is passed into GetGlobalContext. If you
        /// need to modify the global context call SetGobalContext again.</remarks>
        public static void SetGlobalContext(Dictionary<Type, object> context)
        {
            globalContext = context;
            globalContextChanged?.Invoke(null, EventArgs.Empty);
        }

        void Initialize()
        {
            FocusRingType = NSFocusRingType.None;
            CanDrawConcurrently = false;

            stateManager = new(BeginInvokeOnMainThread);
            globalContextChanged += OnGlobalContextChanged;
            rootLayer = new();
            rootLayer.Frame = Bounds;
            AddSubview(rootLayer);
            root = new(CreateContext(), () => new Dummy());
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                globalContextChanged -= OnGlobalContextChanged;
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

            Dictionary<Type, object> context = new()
            {
                { typeof(DpiScale), new DpiScale(scale, scale) },
                { typeof(IFontManager), fontManager },
                { typeof(TextTheme), textTheme },
                { typeof(FlowDirection), flowDirection }
            };

            var currentGlobal = globalContext;
            foreach (var (key, value) in currentGlobal)
                context[key] = value;

            return new(stateManager, rootLayer, context);
        }

        void Root_RootNodeChanged(object sender, EventArgs e) =>
            rootLayer.Root = root.RootNode;

        void OnGlobalContextChanged(object sender, EventArgs e) =>
            BeginInvokeOnMainThread(() => root.Context = CreateContext());
    }
}
