using AppKit;
using Foundation;
using Microsoft.StandardUI.Cocoa;

namespace Interop.Cocoa
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
            // By updating the GlobalContext you can change fonts/colors/ect for all content
            // inside NSStandardUIHost.
            NSStandardUIHost.SetGlobalContext(new()
            {
                { typeof(Style), new Style(HeaderFont: NSFont.BoldSystemFontOfSize(13)) }
            });
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
