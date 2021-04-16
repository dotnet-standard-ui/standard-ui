using AppKit;
using Microsoft.StandardUI.Cocoa;

namespace Interop.Cocoa
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
