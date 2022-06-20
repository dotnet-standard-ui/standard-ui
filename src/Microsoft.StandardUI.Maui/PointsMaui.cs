using Microsoft.StandardUI.XamarinForms.Converters;
using System.ComponentModel;


namespace Microsoft.StandardUI.XamarinForms
{
    [TypeConverter(typeof(PointsTypeConverter))]
    public struct PointsXamarinForms
    {
        public static readonly PointsXamarinForms Default = new PointsXamarinForms(Points.Default);


        public Points Points { get; }

        public PointsXamarinForms(Points points)
        {
            Points = points;
        }
    }
}
