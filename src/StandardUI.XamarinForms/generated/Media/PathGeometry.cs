// This file is generated from IPathGeometry.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class PathGeometry : Geometry, IPathGeometry
    {
        public static readonly BindableProperty FiguresProperty = PropertyUtils.Register(nameof(Figures), typeof(IEnumerable<IPathFigure>), typeof(PathGeometry), null);
        public static readonly BindableProperty FillRuleProperty = PropertyUtils.Register(nameof(FillRule), typeof(FillRule), typeof(PathGeometry), FillRule.EvenOdd);
        
        public IEnumerable<IPathFigure> Figures
        {
            get => (IEnumerable<IPathFigure>) GetValue(FiguresProperty);
            set => SetValue(FiguresProperty, value);
        }
        
        public FillRule FillRule
        {
            get => (FillRule) GetValue(FillRuleProperty);
            set => SetValue(FillRuleProperty, value);
        }
    }
}
