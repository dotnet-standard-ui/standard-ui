using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CoreGraphics;
using Microsoft.StandardUI.Cocoa.Native.Internal;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Tree;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract partial class NSViewBase<TSubclass, TView> : UnsafeControl<Internal.ViewState> where TView : AppKit.NSView, new()
    {
        protected readonly ImmutableDictionary<IUIProperty, object> propertyValues = ImmutableDictionary<IUIProperty, object>.Empty;
        protected readonly ImmutableDictionary<EventInfo, (IListenerFactory, object)> events = ImmutableDictionary<EventInfo, (IListenerFactory, object)>.Empty;
        protected readonly ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures = ImmutableList<(AppKit.NSGestureRecognizer, object)>.Empty;
        protected readonly Size size = new(float.NaN, float.NaN);

        public TSubclass AddGesture(AppKit.NSGestureRecognizer gesture, object key = null)
        {
            var gestures = this.gestures.Add((gesture, key));
            return With_gestures(gestures);
        }

        public TSubclass On<TEventHandler, TArgs>(ListenerFactory<TEventHandler, TArgs> targetEvent, Action<object, TArgs> callback)
        {
            var events = this.events.Add(targetEvent.EventInfo, (targetEvent, callback));
            return With_events(events);
        }

        public TSubclass Set(IUIProperty property, object value)
        {
            var propertyValues = this.propertyValues.SetItem(property, value);
            return With_propertyValues(propertyValues);
        }

        public TSubclass Width(float width) =>
            With_size(new(width, size.Height));

        public TSubclass Height(float height) =>
            With_size(new(size.Width, height));

        public override Element Build(Context context, Internal.ViewState state)
        {
            return new NativeCocoa<TView>(
                () => new(),
                view => Update(view, state),
                ComputeSize);
        }

        public override Node CreateNode(Node parent, Context context)
        {
            return base.CreateNode(parent, context);
        }

        protected virtual void Update(TView view, Internal.ViewState state)
        {
            if (state.Restore == null)
            {
                // First time. No diff.
                Dictionary<IUIProperty, object> restore = new();
                foreach (var (property, value) in propertyValues)
                {
                    restore[property] = property.GetDefault(view);
                    property.Set(view, value);
                }

                Dictionary<object, AppKit.NSGestureRecognizer> knownGestures = new();
                foreach (var (gesture, key) in gestures)
                {
                    view.AddGestureRecognizer(gesture);
                    if (key != null)
                        knownGestures[key] = gesture;
                }

                Dictionary<EventInfo, (IEventListener, object)> events = new();
                foreach (var (eventInfo, rest) in this.events)
                {
                    var (factory, callback) = rest;
                    var listener = factory.Add(state, view);
                    events[eventInfo] = (listener, callback);
                }

                state.Restore = restore;
                state.KnownGestures = knownGestures;
                state.EventTriggers = events;
            }
            else
            {
                // Restore properties that are no longer set
                foreach (var (property, defaultValue) in state.Restore)
                {
                    if (!propertyValues.ContainsKey(property))
                        property.Set(view, defaultValue);
                }

                // Set/update new or changed properties
                Dictionary<IUIProperty, object> restore = new();
                foreach (var (property, value) in propertyValues)
                {
                    property.Update(view, value, state.Restore, restore);
                }

                // Add new gestures. Only track gestures with a non-null key. Gestures with a null key are assumed
                // to only be set once.
                Dictionary<object, AppKit.NSGestureRecognizer> knownGestures = new();
                foreach (var (gesture, key) in gestures)
                {
                    if (key == null)
                        continue;

                    if (state.KnownGestures.TryGetValue(key, out var oldGesture))
                    {
                        knownGestures[key] = oldGesture;
                        continue;
                    }

                    view.AddGestureRecognizer(gesture);
                    knownGestures[key] = gesture;
                }

                foreach (var (key, oldGesture) in state.KnownGestures)
                {
                    if (!knownGestures.ContainsKey(key))
                        view.RemoveGestureRecognizer(oldGesture);
                }

                // Now we update events. Key thing here is each time an event is fired, our listener
                // does a lookup in EventTriggers to find the callback. So we do the following
                // 1. Make sure we're registered for the right events (add/remove)
                // 2. Update EventTriggers to contain the current event callback
                //
                // Design meets the following requirements
                // 1. No invoking callbacks twice
                // 2. No "gap" in coverage when updating events
                Dictionary<EventInfo, (IEventListener, object)> events = new();
                foreach (var (eventInfo, rest) in this.events)
                {
                    var (factory, callback) = rest;
                    if (state.EventTriggers.TryGetValue(eventInfo, out var existing))
                    {
                        var (listener, _) = existing;
                        events[eventInfo] = (listener, callback);
                    }
                    else
                    {
                        var listener = factory.Add(state, view);
                        events[eventInfo] = (listener, callback);
                    }
                }

                foreach (var (eventInfo, rest) in state.EventTriggers)
                {
                    if (!events.ContainsKey(eventInfo))
                    {
                        var (listener, _) = rest;
                        listener.Remove();
                    }
                }

                state.Restore = restore;
                state.KnownGestures = knownGestures;
                state.EventTriggers = events;
            }
        }

        static CGSize IntrinsicSize(TView view)
        {
            var size = view.IntrinsicContentSize;
            var insets = view.AlignmentRectInsets;
            size.Width += insets.Left + insets.Right;
            size.Height += insets.Top + insets.Bottom;
            return size;
        }

        CGSize ComputeSize(TView view, Size availableSize)
        {
            double width;
            double height;
            CGSize? intrinsicSize = null;
            if (float.IsNaN(size.Width))
            {
                intrinsicSize = IntrinsicSize(view);
                width = intrinsicSize.Value.Width;
            }
            else if (float.IsInfinity(size.Width))
                width = availableSize.Width;
            else
                width = size.Width;

            if (float.IsNaN(size.Height))
            {
                intrinsicSize ??= IntrinsicSize(view);
                height = intrinsicSize.Value.Height;
            }
            else if (float.IsInfinity(size.Height))
                height = availableSize.Height;
            else
                height = size.Height;
            return new(width, height);
        }
    }

    namespace Internal
    {
        public interface IUIProperty
        {
            object GetDefault(object instance);
            void Set(object instance, object value);
            void Update(object instance, object value, Dictionary<IUIProperty, object> previousDefault, Dictionary<IUIProperty, object> nextDefault);
        }

        public class UIProperty : IUIProperty
        {
            private PropertyInfo property;

            public UIProperty(PropertyInfo propertyInfo) => this.property = propertyInfo;

            public object GetDefault(object instance) =>
                property.GetValue(instance);

            public void Set(object instance, object value) =>
                property.SetValue(instance, value);

            public void Update(object instance, object value,
                Dictionary<IUIProperty, object> previousDefault,
                Dictionary<IUIProperty, object> nextDefault)
            {
                // TODO No support for set only values consideration for values that get regenerated on every get.
                // Consider option to disable the "Equals" check. We could be duplicating some work here. It would
                // also make the implementation significantly simpler
                var currentValue = property.GetValue(instance);
                if (!Equals(currentValue, value))
                {
                    if (!previousDefault.TryGetValue(this, out var defaultValue))
                        defaultValue = GetDefault(instance);
                    nextDefault[this] = defaultValue;
                    Set(instance, value);
                }
            }

            public override int GetHashCode() => property.GetHashCode();

            public override bool Equals(object obj) =>
                obj is UIProperty uip && uip.property.Equals(property);
        }

        public class CustomUIProperty<TView, T> : IUIProperty
        {
            private T defaultValue;
            private Action<TView, T> setValue;

            public CustomUIProperty(T defaultValue, Action<TView, T> setValue)
            {
                this.defaultValue = defaultValue;
                this.setValue = setValue;
            }

            public object GetDefault(object instance) =>
                defaultValue;

            public void Set(object instance, object value) =>
                setValue((TView)instance, (T)value);

            public void Update(object instance, object value,
                Dictionary<IUIProperty, object> previousDefault,
                Dictionary<IUIProperty, object> nextDefault)
            {
                nextDefault[this] = defaultValue;
                Set(instance, value);
            }
        }

        public struct ValueInfo
        {
            public ValueInfo(object value)
            {
                IsDefaultKnown = false;
                Default = null;
                Value = value;
                SetValue = DefaultSet;
            }

            public ValueInfo(object value, object defaultValue)
            {
                IsDefaultKnown = true;
                Default = defaultValue;
                Value = value;
                SetValue = DefaultSet;
            }

            public ValueInfo(object value, object defaultValue, Action<object, PropertyInfo, object> setOverride)
            {
                IsDefaultKnown = true;
                Default = defaultValue;
                Value = value;
                SetValue = setOverride;
            }

            public bool IsDefaultKnown;
            public object Default;
            public object Value;
            public Action<object, PropertyInfo, object> SetValue;

            public object GetDefault(object view, PropertyInfo property)
            {
                object defaultValue = null;
                if (IsDefaultKnown)
                    defaultValue = Default;
                else if (property.CanRead && view != null)
                    defaultValue = property.GetValue(view);
                else if (property.PropertyType.IsValueType)
                    defaultValue = Activator.CreateInstance(property.PropertyType);
                return defaultValue;
            }

            static void DefaultSet(object obj, PropertyInfo prop, object value) =>
                prop.SetValue(obj, value);
        }

        public class ViewState : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged
            {
                add { }
                remove { }
            }

            public Dictionary<IUIProperty, object> Restore;
            public Dictionary<object, AppKit.NSGestureRecognizer> KnownGestures;

            /// <summary>
            /// Maps EventInfo to the IEventListener and callback
            /// </summary>
            /// <remarks>
            /// The IEventListener is used to remove the event when it's no longer needed. The
            /// callback is a Func with the right type for the given event.
            /// </remarks>
            public Dictionary<EventInfo, (IEventListener, object)> EventTriggers;
        }

        public interface IListenerFactory
        {
            EventInfo EventInfo { get; }
            IEventListener Add(ViewState state, object view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEventHandler">Type of the underly event handler</typeparam>
        /// <typeparam name="TEventArgs">The type of the handler's event argument</typeparam>
        public class ListenerFactory<TEventHandler, TEventArgs> : IListenerFactory
        {
            public ListenerFactory(EventInfo eventInfo)
            {
                Debug.Assert(eventInfo != null, "EventInfo must be non-null");
                Debug.Assert(eventInfo.EventHandlerType == typeof(TEventHandler),
                    $"Listener factory should be declared with {eventInfo.EventHandlerType.Name}");
                EventInfo = eventInfo;
            }

            public EventInfo EventInfo { get; }

            public IEventListener Add(ViewState state, object view)
            {
                EventListener<TEventHandler, TEventArgs> listener = new(EventInfo, state, view);
                return listener;
            }
        }

        public interface IEventListener
        {
            void Remove();
        }

        public class EventListener<TEventHandler, TArgs> : IEventListener
        {
            EventInfo eventInfo;
            ViewState state;
            object target;
            Delegate handler;

            public EventListener(EventInfo eventInfo, ViewState state, object target)
            {
                this.eventInfo = eventInfo;
                this.state = state;
                this.target = target;

                handler = Delegate.CreateDelegate(typeof(TEventHandler), this, "OnEvent");
                eventInfo.AddEventHandler(target, handler);
            }

            public void Remove() =>
                eventInfo.RemoveEventHandler(target, handler);

            public void OnEvent(object sender, TArgs args)
            {
                var triggers = state.EventTriggers;
                if (triggers == null)
                    return;

                if (triggers.TryGetValue(eventInfo, out var value))
                {
                    var (_, trigger) = value;
                    var actualTrigger = (Action<object, TArgs>)trigger;
                    actualTrigger(sender, args);
                }
            }
        }
    }
}
