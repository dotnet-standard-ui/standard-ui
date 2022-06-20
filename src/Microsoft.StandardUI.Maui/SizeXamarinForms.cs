using System.ComponentModel;
using Microsoft.StandardUI.XamarinForms.Converters;

namespace Microsoft.StandardUI.XamarinForms
{
    [TypeConverter(typeof(SizeTypeConverter))]
    public struct SizeXamarinForms
    {
        public static readonly SizeXamarinForms Default = new SizeXamarinForms(Size.Default);


        public Size Size { get; }

        public SizeXamarinForms(Size size)
        {
            Size = size;
        }
    }
}
