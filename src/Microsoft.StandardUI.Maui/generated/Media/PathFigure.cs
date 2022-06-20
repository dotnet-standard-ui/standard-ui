// This file is generated from IPathFigure.cs. Update the source file to change its contents.

using System.Collections.Generic;
using Microsoft.StandardUI.Media;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class PathFigure : StandardUIBindableObject, IPathFigure
    {
        public static readonly BindableProperty SegmentsProperty = PropertyUtils.Register(nameof(Segments), typeof(IEnumerable<IPathSegment>), typeof(PathFigure), null);
        public static readonly BindableProperty StartPointProperty = PropertyUtils.Register(nameof(StartPoint), typeof(PointXamarinForms), typeof(PathFigure), PointXamarinForms.Default);
        public static readonly BindableProperty IsClosedProperty = PropertyUtils.Register(nameof(IsClosed), typeof(bool), typeof(PathFigure), false);
        public static readonly BindableProperty IsFilledProperty = PropertyUtils.Register(nameof(IsFilled), typeof(bool), typeof(PathFigure), true);
        
        public IEnumerable<IPathSegment> Segments
        {
            get => (IEnumerable<IPathSegment>) GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }
        
        public PointXamarinForms StartPoint
        {
            get => (PointXamarinForms) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point IPathFigure.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointXamarinForms(value);
        }
        
        public bool IsClosed
        {
            get => (bool) GetValue(IsClosedProperty);
            set => SetValue(IsClosedProperty, value);
        }
        
        public bool IsFilled
        {
            get => (bool) GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, value);
        }
    }
}
