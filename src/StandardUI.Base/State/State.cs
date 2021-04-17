using Microsoft.StandardUI.Elements;
using System;
using System.ComponentModel;
using System.Threading;

namespace Microsoft.StandardUI.State
{
    public static class State
    {
        public static InjectState<State<T>> Inject<T>(Func<T, Action<T>, Element> callback) where T : new() =>
            new InjectState<State<T>>(() => new(), state => callback(state.Value, newState => state.Value = newState));
        public static InjectState<State<T>> Inject<T>(Func<T> newState, Func<T, Action<T>, Element> callback) =>
            new InjectState<State<T>>(() => new State<T>(newState()),
                state => callback(state.Value, newState => state.Value = newState));
        public static UnsafeInjectState<T> UnsafeInject<T>(Func<T, Element> callback) where T : new() =>
            new UnsafeInjectState<T>(() => new(), callback);
        public static UnsafeInjectState<T> UnsafeInject<T>(Func<T> newState, Func<T, Element> callback) =>
            new UnsafeInjectState<T>(() => new State<T>(newState()), callback);
    }

    public sealed class State<T> : INotifyPropertyChanged, IDisposable
    {
        private int disposed;
        public event PropertyChangedEventHandler? PropertyChanged;

        private T v;

        public static implicit operator T(State<T> state) => state.Value;

        public State() => this.v = Activator.CreateInstance<T>();

        public State(T v) => this.v = v;

        // An implicit conversion operator would be useful. Unforunately it allows the following:
        // State<bool> foo = false;
        // foo = true;
        //
        // This ends up creating two states, but 99% of the time you really want
        // foo.Value = true;

        public T Value
        {
            get => v;
            set
            {
                v = value;
                PropertyChanged?.Invoke(this, PropertyChangedHelper.All);
            }
        }

        public void Dispose()
        {
            if (v is IDisposable d && Interlocked.CompareExchange(ref disposed, 1, 0) == 0)
                d.Dispose();
        }

        public override string ToString() => Value?.ToString() ?? "";
    }
}
