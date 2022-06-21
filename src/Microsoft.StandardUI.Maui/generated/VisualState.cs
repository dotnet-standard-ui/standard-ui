// This file is generated from IVisualState.cs. Update the source file to change its contents.

using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui
{
    public class VisualState : StandardUIObject, IVisualState
    {
        public static readonly BindableProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualState), "");
        public static readonly BindableProperty SettersProperty = PropertyUtils.Register(nameof(Setters), typeof(UICollection<ISetter>), typeof(VisualState), null);
        
        private UICollection<ISetter> _setters;
        
        public VisualState()
        {
            _setters = new UICollection<ISetter>(this);
            SetValue(SettersProperty, _setters);
        }
        
        public string Name => (string) GetValue(NameProperty);
        
        public UICollection<ISetter> Setters => _setters;
        IUICollection<ISetter> IVisualState.Setters => Setters;
    }
}
