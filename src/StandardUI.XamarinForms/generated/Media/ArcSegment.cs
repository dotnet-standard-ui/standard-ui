// This file is generated from IArcSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using SweepDirection = Microsoft.StandardUI.Media.SweepDirection;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Media
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public static readonly BindableProperty PointProperty = PropertyUtils.Register(nameof(Point), typeof(PointXamarinForms), typeof(ArcSegment), PointXamarinForms.Default);
        public static readonly BindableProperty SizeProperty = PropertyUtils.Register(nameof(Size), typeof(SizeXamarinForms), typeof(ArcSegment), SizeXamarinForms.Default);
        public static readonly BindableProperty RotationAngleProperty = PropertyUtils.Register(nameof(RotationAngle), typeof(double), typeof(ArcSegment), 0.0);
        public static readonly BindableProperty IsLargeArcProperty = PropertyUtils.Register(nameof(IsLargeArc), typeof(bool), typeof(ArcSegment), false);
        public static readonly BindableProperty SweepDirectionProperty = PropertyUtils.Register(nameof(SweepDirection), typeof(SweepDirection), typeof(ArcSegment), SweepDirection.Counterclockwise);
        
        public PointXamarinForms Point
        {
            get => (PointXamarinForms) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point IArcSegment.Point
        {
            get => Point.Point;
            set => Point = new PointXamarinForms(value);
        }
        
        public SizeXamarinForms Size
        {
            get => (SizeXamarinForms) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        Size IArcSegment.Size
        {
            get => Size.Size;
            set => Size = new SizeXamarinForms(value);
        }
        
        public double RotationAngle
        {
            get => (double) GetValue(RotationAngleProperty);
            set => SetValue(RotationAngleProperty, value);
        }
        
        public bool IsLargeArc
        {
            get => (bool) GetValue(IsLargeArcProperty);
            set => SetValue(IsLargeArcProperty, value);
        }
        
        public SweepDirection SweepDirection
        {
            get => (SweepDirection) GetValue(SweepDirectionProperty);
            set => SetValue(SweepDirectionProperty, value);
        }
    }
}
