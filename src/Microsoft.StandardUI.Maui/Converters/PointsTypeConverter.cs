using Microsoft.StandardUI.Converters;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.Maui.Converters
{
	public class PointsTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object valueObject)
        {
            return new PointsMaui(PointsConverter.ConvertFromString(GetValueAsString(valueObject)));
        }
    }
}
