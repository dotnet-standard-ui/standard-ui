using Microsoft.StandardUI.Text;

namespace Microsoft.StandardUI.XamarinForms.Text
{
    public struct FontWeightXamarinForms
    {
        public static readonly FontWeightXamarinForms Default = new FontWeightXamarinForms(FontWeights.Normal);

        public static FontWeightXamarinForms FromFontWeight(FontWeight fontWeight) => new FontWeightXamarinForms(fontWeight);

        // Auto properties
        public FontWeight FontWeight { get; }

        public FontWeightXamarinForms(FontWeight fontWeight)
        {
            FontWeight = fontWeight;
        }
    }
}
