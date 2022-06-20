using System.ComponentModel;
using Microsoft.StandardUI.XamarinForms.Converters;

namespace Microsoft.StandardUI.XamarinForms
{
    [TypeConverter(typeof(ColorTypeConverter))]
    public struct ColorXamarinForms
    {
        public static readonly ColorXamarinForms Default = new ColorXamarinForms(Microsoft.StandardUI.Color.Default);
        public static readonly ColorXamarinForms Transparent = new ColorXamarinForms(Colors.Transparent);

        public static ColorXamarinForms FromColor(Color color) => new ColorXamarinForms(color);

        // Auto properties
        public Color Color { get; }

        public ColorXamarinForms(Color color)
        {
            Color = color;
        }
    }
}
