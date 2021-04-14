using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.StandardUI.Cocoa.Native.Internal;
using Microsoft.StandardUI.Elements;
using Microsoft.StandardUI.Tree;

namespace Microsoft.StandardUI.Cocoa.Native
{
    public abstract partial class NSView<TSubclass, TView> : UnsafeControl<Internal.ViewState> where TView : AppKit.NSView, new()
    {
        public NSView(
            ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues,
            ImmutableDictionary<EventInfo, (IListenerFactory, object)> events,
            ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures)
        {
            PropertyValues = propertyValues;
            Events = events;
            Gestures = gestures;
        }

        public ImmutableDictionary<PropertyInfo, ValueInfo> PropertyValues { get; }
        public ImmutableDictionary<EventInfo, (IListenerFactory, object)> Events { get; }
        public ImmutableList<(AppKit.NSGestureRecognizer, object)> Gestures { get; }

        public TSubclass AddGesture(AppKit.NSGestureRecognizer gesture, object key = null)
        {
            var gestures = Gestures.Add((gesture, key));
            return WithGestures(gestures);
        }

        public TSubclass On<TEventHandler, TArgs>(ListenerFactory<TEventHandler, TArgs> targetEvent, Action<object, TArgs> callback)
        {
            var events = Events.Add(targetEvent.EventInfo, (targetEvent, callback));
            return WithEvents(events);
        }

        public TSubclass Set(string propertyName, object value) =>
            Set(propertyName, new ValueInfo(value));

        public TSubclass Set(string propertyName, object value, object defaultValue) =>
            Set(propertyName, new ValueInfo(value, defaultValue));

        public TSubclass Set(string propertyName, ValueInfo value)
        {
            var property = typeof(TSubclass).GetProperty(propertyName);
            return Set(property, value);
        }

        public TSubclass Set(PropertyInfo property, object value) =>
            Set(property, new ValueInfo(value));

        public TSubclass Set(PropertyInfo property, object value, object defaultValue) =>
            Set(property, new ValueInfo(value, defaultValue));

        public TSubclass Set(PropertyInfo property, ValueInfo value)
        {
            var propertyValues = PropertyValues.SetItem(property, value);
            return WithProperties(propertyValues);
        }

        public override Element Build(Context context, Internal.ViewState state)
        {
            return new NativeCocoa<TView>(
                () => new(),
                view => Update(view, state));
        }

        public override Node CreateNode(Node parent, Context context)
        {
            return base.CreateNode(parent, context);
        }

        protected abstract TSubclass WithEvents(ImmutableDictionary<EventInfo, (IListenerFactory, object)> events);
        protected abstract TSubclass WithGestures(ImmutableList<(AppKit.NSGestureRecognizer, object)> gestures);
        protected abstract TSubclass WithProperties(ImmutableDictionary<PropertyInfo, ValueInfo> propertyValues);

        void Update(TView view, Internal.ViewState state)
        {
            if (state.Restore == null)
            {
                // First time. No diff.
                Dictionary<PropertyInfo, object> restore = new();
                foreach (var (property, value) in PropertyValues)
                {
                    restore[property] = value.GetDefault(view, property);
                    property.SetValue(view, value.Value);
                }

                Dictionary<object, AppKit.NSGestureRecognizer> knownGestures = new();
                foreach (var (gesture, key) in Gestures)
                {
                    view.AddGestureRecognizer(gesture);
                    if (key != null)
                        knownGestures[key] = gesture;
                }

                Dictionary<EventInfo, (IEventListener, object)> events = new();
                foreach (var (eventInfo, rest) in Events)
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
                foreach (var (propertyInfo, defaultValue) in state.Restore)
                {
                    if (!PropertyValues.ContainsKey(propertyInfo))
                        propertyInfo.SetValue(view, defaultValue);
                }

                // Set/update new or changed properties
                Dictionary<PropertyInfo, object> restore = new();
                foreach (var (property, value) in PropertyValues)
                {
                    // TODO No support for set only values consideration for values that get regenerated on every get.
                    // Consider option to disable the "Equals" check. We could be duplicating some work here.
                    var currentValue = property.GetValue(view);
                    if (!Equals(currentValue, value))
                    {
                        if (!state.Restore.TryGetValue(property, out var defaultValue))
                            defaultValue = value.GetDefault(view, property);
                        restore[property] = defaultValue;
                        property.SetValue(view, value.Value);
                    }
                }

                // Add new gestures. Only track gestures with a non-null key. Gestures with a null key are assumed
                // to only be set once.
                Dictionary<object, AppKit.NSGestureRecognizer> knownGestures = new();
                foreach (var (gesture, key) in Gestures)
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
                foreach (var (eventInfo, rest) in Events)
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
    }

    namespace Internal
    {
        public struct ValueInfo
        {
            public ValueInfo(object value)
            {
                IsDefaultKnown = false;
                Default = null;
                Value = value;
            }

            public ValueInfo(object value, object defaultValue)
            {
                IsDefaultKnown = true;
                Default = defaultValue;
                Value = value;
            }

            public bool IsDefaultKnown;
            public object Default;
            public object Value;

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
        }

        public class ViewState : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged
            {
                add { }
                remove { }
            }

            public Dictionary<PropertyInfo, object> Restore;
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
