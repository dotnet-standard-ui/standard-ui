using System;
using CoreGraphics;

namespace Microsoft.StandardUI.Cocoa
{
    public static class Extensions
    {
        public static CGSize Into(this Size size) => new(size.Width, size.Height);
        public static Size Into(this CGSize size) => new((float)size.Width, (float)size.Height);
    }
}
