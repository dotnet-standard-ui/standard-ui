using Microsoft.StandardUI.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.StandardUI.Elements
{
    public abstract class InjectStateBase<TState> : Element
    {
        public InjectStateBase(Func<TState> newState, Func<TState, Element> callback)
        {
            NewState = newState;
            Callback = callback;
        }

        public Func<TState> NewState { get; }
        public Func<TState, Element> Callback { get; }

        public abstract void AddListener(TState state, PropertyChangedEventHandler handler);
        public abstract void RemoveListener(TState state, PropertyChangedEventHandler handler);

        public override Node CreateNode(Node? parent, Context context) =>
            new InjectStateNode<TState>(parent, context, this);
    }

    public class InjectState<TState> : InjectStateBase<TState> where TState : INotifyPropertyChanged
    {
        public InjectState(Func<TState> newState, Func<TState, Element> callback) : base(newState, callback)
        { }

        public override void AddListener(TState state, PropertyChangedEventHandler handler) =>
            state.PropertyChanged += handler;

        public override void RemoveListener(TState state, PropertyChangedEventHandler handler) =>
            state.PropertyChanged -= handler;
    }

    public class UnsafeInjectState<TState> : InjectStateBase<TState>
    {
        public UnsafeInjectState(Func<TState> newState, Func<TState, Element> callback) : base(newState, callback)
        { }

        public override void AddListener(TState state, PropertyChangedEventHandler handler) { }
        public override void RemoveListener(TState state, PropertyChangedEventHandler handler) { }
    }

    class InjectStateNode<TState> : NodeBase<InjectStateBase<TState>>
    {
        TState state;
        Node child;

        public InjectStateNode(Node? parent, Context context, InjectStateBase<TState> element) : base(parent, context, element)
        {
            state = element.NewState();
            element.AddListener(state, State_PropertyChanged);

            var childElement = element.Callback(state);
            child = childElement.CreateNode(this, context);
        }

        public override IEnumerable<Node> Children
        {
            get
            {
                yield return child;
            }
        }

        protected override (Size, float?) ArrangeOverride(Size availableSize) => child.Arrange(availableSize);

        public override void Dispose()
        {
            Element.RemoveListener(state, State_PropertyChanged);
            (state as IDisposable)?.Dispose();
            base.Dispose();
        }

        protected override void UpdateElement(InjectStateBase<TState> oldElement, Context oldContext) => UpdateState();

        void UpdateState()
        {
            var newChild = Element.Callback(state);
            child = child.UpdateElement(newChild, Context);
        }

        void State_PropertyChanged(object? sender, EventArgs e) =>
            Context.InvalidateState(Depth, UpdateState);
    }
}
