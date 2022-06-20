using Microsoft.StandardUI.Converters;
using Microsoft.StandardUI.XamarinForms.Media;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.XamarinForms.Converters
{
	public class BrushTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new SolidColorBrush
            {
                Color = new ColorXamarinForms(ColorConverter.ConvertFromString(GetValueAsString(valueObject)))
            };
        }
	}
}
