namespace Microsoft.StandardUI.Elements
{
    /// <summary>
    /// Enables record based Controls
    /// </summary>
    /// <remarks>
    /// Record types can not inherit from class types. Since Element is a class, we can not have
    /// record Elements. RControl is a record that is implicitly convertable to Element. This lets
    /// you use records to write your UI.
    /// </remarks>
    public abstract record RControl
    {
        public static implicit operator Element(RControl control) =>
            new RControlAdapter(control);

        public abstract Element Build(Context context);
    }

    class RControlAdapter : Control
    {
        RControl inner;

        public RControlAdapter(RControl inner) =>
            this.inner = inner;

        public override Element Build(Context context) =>
            inner.Build(context);
    }
}
