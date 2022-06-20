using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.XamarinForms
{
    public class XamarinFormsStandardUIEnvironment : IStandardUIEnvironment
    {
        private XamarinFormsStandardUIFactory _uiElementFactory = new XamarinFormsStandardUIFactory();
        private IVisualEnvironment _visualEnvironment;

        public static void Init(IVisualEnvironment visualEnvironment)
        {
            StandardUIEnvironment.Init(new XamarinFormsStandardUIEnvironment(visualEnvironment));
        }

        public XamarinFormsStandardUIEnvironment(IVisualEnvironment visualEnvironment)
        {
            _visualEnvironment = visualEnvironment;
        }

        public IVisualEnvironment VisualEnvironment => _visualEnvironment;

        public IStandardUIFactory Factory => _uiElementFactory;
    }
}
