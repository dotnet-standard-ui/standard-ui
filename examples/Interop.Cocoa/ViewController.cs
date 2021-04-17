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

            // Generate some initial data. This would normally be read from configuration.
            OptionsData data = new(
                Configurations: new[] { "Debug (Active)", "Release" },
                SelectedConfiguration: new(0),
                Platforms: new[] { "Any CPU" },
                SelectedPlatform: new(0),
                GenerateOverflowChecks: new(false),
                EnableOptimizations: new(false),
                GenerateXmlDoc: new(false),
                XmlDocPath: new("Interop.Cocoa.xml"),
                Symbols: new("__MACOS__;__UNDEFINED__;DEBUG;"),
                PlatformTargets: new[] { "Any CPU", "x86", "x64", "Itanium" },
                SelectedPlatformTarget: new(0)
                );
            host.RootElement = () => new OptionsPane(
                data,
                Ok: () => Debug.WriteLine($"Ok {data}"),
                Cancel: () => Debug.WriteLine($"Cancel {data}"));
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
