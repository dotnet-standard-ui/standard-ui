using System.ComponentModel;
using System.Globalization;

namespace Microsoft.StandardUI.XamarinForms.Converters
{
	public class GeometryTypeConverter : TypeConverterBase
	{
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object valueObject)
        {
            ///return PathConverter.ParsePathGeometry(GetValueAsString(valueObject), GeometryFactory.Instance);
            return "";
        }
	}
}
