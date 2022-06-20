using Microsoft.StandardUI.Converters;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.XamarinForms.Converters
{
	public class ColorTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            return new ColorXamarinForms(ColorConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
	}
}
