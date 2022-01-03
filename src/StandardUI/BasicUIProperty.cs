namespace Microsoft.StandardUI
{
    public class BasicUIProperty : IUIProperty
    {
        private string _name;
        private object? _defaultValue;
        private bool _readOnly;

        public BasicUIProperty(string name, object? defaultValue, bool readOnly = false)
        {
            this._name = name;
            this._defaultValue = defaultValue;
            this._readOnly = readOnly;
        }

        public string Name => _name;

        public object? DefaultValue => _defaultValue;

        public bool ReadOnly => _readOnly;
    }
}