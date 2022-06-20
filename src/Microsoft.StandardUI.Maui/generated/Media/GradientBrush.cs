// This file is generated from IGradientBrush.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class GradientBrush : Brush, IGradientBrush
    {
        public static readonly BindableProperty GradientStopsProperty = PropertyUtils.Register(nameof(GradientStops), typeof(IEnumerable<IGradientStop>), typeof(GradientBrush), null);
        public static readonly BindableProperty MappingModeProperty = PropertyUtils.Register(nameof(MappingMode), typeof(BrushMappingMode), typeof(GradientBrush), BrushMappingMode.RelativeToBoundingBox);
        public static readonly BindableProperty SpreadMethodProperty = PropertyUtils.Register(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(GradientBrush), GradientSpreadMethod.Pad);
        
        public IEnumerable<IGradientStop> GradientStops
        {
            get => (IEnumerable<IGradientStop>) GetValue(GradientStopsProperty);
            set => SetValue(GradientStopsProperty, value);
        }
        
        public BrushMappingMode MappingMode
        {
            get => (BrushMappingMode) GetValue(MappingModeProperty);
            set => SetValue(MappingModeProperty, value);
        }
        
        public GradientSpreadMethod SpreadMethod
        {
            get => (GradientSpreadMethod) GetValue(SpreadMethodProperty);
            set => SetValue(SpreadMethodProperty, value);
        }
    }
}
