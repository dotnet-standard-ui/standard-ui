using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms
{
    public class UIProperty : IUIProperty
    {
        public BindableProperty BindableProperty { get;  }

        public UIProperty(BindableProperty property)
        {
            BindableProperty = property;
        }

        public static BindableProperty GetBindableProperty(IUIProperty property)
        {
            return ((UIProperty)property).BindableProperty;
        }
    }
}
