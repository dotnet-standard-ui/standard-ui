// This file is generated from IShape.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.XamarinForms.Media;
using Brush = Microsoft.StandardUI.XamarinForms.Media.Brush;
using Microsoft.StandardUI.Shapes;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Shapes
{
    public class Shape : StandardUIView, IShape
    {
        public static readonly BindableProperty FillProperty = PropertyUtils.Register(nameof(Fill), typeof(Brush), typeof(Shape), null);
        public static readonly BindableProperty StrokeProperty = PropertyUtils.Register(nameof(Stroke), typeof(Brush), typeof(Shape), null);
        public static readonly BindableProperty StrokeThicknessProperty = PropertyUtils.Register(nameof(StrokeThickness), typeof(double), typeof(Shape), 1.0);
        public static readonly BindableProperty StrokeMiterLimitProperty = PropertyUtils.Register(nameof(StrokeMiterLimit), typeof(double), typeof(Shape), 10.0);
        public static readonly BindableProperty StrokeLineCapProperty = PropertyUtils.Register(nameof(StrokeLineCap), typeof(PenLineCap), typeof(Shape), PenLineCap.Flat);
        public static readonly BindableProperty StrokeLineJoinProperty = PropertyUtils.Register(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(Shape), PenLineJoin.Miter);
        
        public Brush? Fill
        {
            get => (Brush?) GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        IBrush? IShape.Fill
        {
            get => Fill;
            set => Fill = (Brush?) value;
        }
        
        public Brush? Stroke
        {
            get => (Brush?) GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }
        IBrush? IShape.Stroke
        {
            get => Stroke;
            set => Stroke = (Brush?) value;
        }
        
        public double StrokeThickness
        {
            get => (double) GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        
        public double StrokeMiterLimit
        {
            get => (double) GetValue(StrokeMiterLimitProperty);
            set => SetValue(StrokeMiterLimitProperty, value);
        }
        
        public PenLineCap StrokeLineCap
        {
            get => (PenLineCap) GetValue(StrokeLineCapProperty);
            set => SetValue(StrokeLineCapProperty, value);
        }
        
        public PenLineJoin StrokeLineJoin
        {
            get => (PenLineJoin) GetValue(StrokeLineJoinProperty);
            set => SetValue(StrokeLineJoinProperty, value);
        }
    }
}
