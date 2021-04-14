using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.StandardUI.Cocoa.Native.Internal;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract class NSButtonBase<TSubclass, TView> : NSControl<TSubclass, TView> where TView : AppKit.NSButton, new()
    {
        public static readonly PropertyInfo BezelStyleProperty = typeof(AppKit.NSButton).GetProperty("BezelStyle");
        public static readonly PropertyInfo TitleProperty = typeof(AppKit.NSButton).GetProperty("Title");

        protected NSButtonBase(ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues, ImmutableDictionary<EventInfo, (IListenerFactory, object)> events, ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures) : base(propertyValues, events, gestures)
        {
        }

        public TSubclass BezelStyle(AppKit.NSBezelStyle bezelStyle) => Set(BezelStyleProperty, bezelStyle);
        public TSubclass Title(string title) => Set(TitleProperty, title);
    }

    public sealed class NSButton : NSButtonBase<NSButton, AppKit.NSButton>
    {
        public NSButton() : this(
            ImmutableDictionary.Create<PropertyInfo, ValueInfo>(),
            ImmutableDictionary.Create<EventInfo, (IListenerFactory, object)>(),
            ImmutableList.Create<(AppKit.NSGestureRecognizer, object)>())
        { }

        private NSButton(ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues, ImmutableDictionary<EventInfo, (IListenerFactory, object)> events, ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures) : base(propertyValues, events, gestures)
        {
        }

        protected override NSButton WithEvents(ImmutableDictionary<EventInfo, (IListenerFactory, object)> events) =>
            new(PropertyValues, events, Gestures);

        protected override NSButton WithGestures(ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures) =>
            new(PropertyValues, Events, gestures);

        protected override NSButton WithProperties(ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues) =>
            new(propertyValues, Events, Gestures);
    }
}
