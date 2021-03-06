// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using DependencyProperty = Microsoft.UI.Xaml.DependencyProperty;

namespace Microsoft.StandardUI.WinUI.Media
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly DependencyProperty PointProperty = PropertyUtils.Register(nameof(Point), typeof(PointWinUI), typeof(LineSegment), PointWinUI.Default);
        
        public PointWinUI Point
        {
            get => (PointWinUI) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point ILineSegment.Point
        {
            get => Point.Point;
            set => Point = new PointWinUI(value);
        }
    }
}
