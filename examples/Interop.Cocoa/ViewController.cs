using System;
using System.Diagnostics;
using AppKit;
using Foundation;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.State;

using Native = Microsoft.StandardUI.Cocoa.Native;

namespace Interop.Cocoa
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // README: Close/reopen solution after building the first time. Workaround until VSMac has source generator support.
            // Testing layout here. A real implementation would have to bind to/update a real view model.
            host.RootElement = () =>
                State.Inject<int>((state, setState) =>
                    new Column(
                        new Row(
                            VerticalAlignment.Center,
                            TextBlock("Configuration:"),
                            new Native.NSPopUpButton()
                                .Items("Debug (Active)", "Release")
                                .Activated(PrintConfiguration),
                            TextBlock("Platform:"),
                            new Native.NSPopUpButton()
                                .Items("Any CPU")
                            ),
                        TextBlock("General Options"),
                        new Column(
                            new Native.NSButton()
                                .Title($"Generate overflow checks {state}")
                                .State(state % 2 == 0 ? NSCellStateValue.Off : NSCellStateValue.On)
                                .ButtonType(NSButtonType.Switch)
                                .Activated(() => setState(state + 1)),
                            new Native.NSButton()
                                .Title("Enable optimizations")
                                .State(NSCellStateValue.Off)
                                .ButtonType(NSButtonType.Switch),
                            new Row(
                                VerticalAlignment.Center,
                                new Native.NSButton()
                                    .Title("Generate xml documentation:")
                                    .State(NSCellStateValue.Off)
                                    .ButtonType(NSButtonType.Switch),
                                new Native.NSTextField()
                                    .StringValue("Interop.Cocoa.xml")
                                    .Changed(() => Debug.WriteLine("xml doc changed"))
                                    .Width(float.PositiveInfinity)
                                    .Expand(),
                                new Native.NSButton()
                                    .Title("Browse...")
                                    .BezelStyle(NSBezelStyle.Rounded)
                                ),
                            new Row(
                                TextBlock("Define Symbols:"),
                                new Native.NSTextField()
                                    .StringValue("__MACOS__;__UNDEFINED__;DEBUG;")
                                    .Width(float.PositiveInfinity)
                                 ),
                            new Row(
                                TextBlock("Platform targets:"),
                                new Native.NSPopUpButton()
                                    .Items("Any CPU", "x86", "x64", "Itanium")
                                )
                            ).Margin(10)
                        )
                    );
        }

        static Native.NSTextField TextBlock(string text) =>
            new Native.NSTextField()
                .StringValue(text)
                .BackgroundColor(NSColor.Clear)
                .Bezeled(false)
                .Editable(false);

        private void PrintConfiguration(object sender, EventArgs e)
        {
            var btn = (NSPopUpButton)sender;
            Debug.WriteLine($"Configuration: {btn.Title}");
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
