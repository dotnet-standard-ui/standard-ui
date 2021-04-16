using System;
using System.Runtime.CompilerServices;
using Microsoft.StandardUI.Cocoa.Native.Internal;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract partial class NSButtonBase<TSubclass, TView>
    {
        public static CustomUIProperty<AppKit.NSButton, AppKit.NSButtonType> ButtonTypeProperty =
            new(AppKit.NSButtonType.MomentaryLightButton, (view, type) => view.SetButtonType(type));

        public TSubclass ButtonType(AppKit.NSButtonType buttonType) =>
            Set(ButtonTypeProperty, buttonType);
    }
}
