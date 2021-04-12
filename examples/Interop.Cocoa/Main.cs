using AppKit;
using Microsoft.StandardUI.Cocoa;

namespace Interop.Cocoa
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSStandardUIHost.Reference();
            NSMyControl.Reference();
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
