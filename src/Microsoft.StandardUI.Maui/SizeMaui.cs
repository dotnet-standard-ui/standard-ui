using System.ComponentModel;
using Microsoft.StandardUI.Maui.Converters;

namespace Microsoft.StandardUI.Maui
{
    [TypeConverter(typeof(SizeTypeConverter))]
    public struct SizeMaui
    {
        public static readonly SizeMaui Default = new SizeMaui(Size.Default);


        public Size Size { get; }

        public SizeMaui(Size size)
        {
            Size = size;
        }
    }
}
