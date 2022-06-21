using Microsoft.StandardUI.Converters;
using Microsoft.StandardUI.Maui.Media;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.Maui.Converters
{
	public class BrushTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object valueObject) =>
            new SolidColorBrush
            {
                Color = new ColorMaui(ColorConverter.ConvertFromString(GetValueAsString(valueObject)))
            };
	}
}
