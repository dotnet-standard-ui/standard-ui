using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using AppKit;
using Microsoft.StandardUI.Cocoa.Native.Internal;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public static class NSControl
    {
        public static readonly Internal.ListenerFactory<EventHandler, EventArgs> ActivatedEvent = new(typeof(AppKit.NSControl).GetEvent("Activated"));
    }

    public abstract class NSControl<TSubclass, TView> : NSView<TSubclass, TView> where TView : AppKit.NSControl, new()
    {

        protected NSControl(ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues, ImmutableDictionary<EventInfo, (IListenerFactory, object)> events, ImmutableList<(NSGestureRecognizer, object)> gestures) : base(propertyValues, events, gestures)
        {
        }

        public TSubclass Activated(Action onActivated) =>
            On(NSControl.ActivatedEvent, (_, _) => onActivated());

        public TSubclass Activated(Action<object, EventArgs> onActivated) =>
            On(NSControl.ActivatedEvent, onActivated);
    }
}
