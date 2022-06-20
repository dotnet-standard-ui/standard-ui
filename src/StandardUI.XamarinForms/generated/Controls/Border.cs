// This file is generated from IBorder.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.XamarinForms.Media;
using Brush = Microsoft.StandardUI.XamarinForms.Media.Brush;
using Microsoft.StandardUI.Controls;
using Xamarin.Forms;

namespace Microsoft.StandardUI.XamarinForms.Controls
{
    public class Border : StandardUIView, IBorder
    {
        public static readonly BindableProperty BackgroundProperty = PropertyUtils.Register(nameof(Background), typeof(Brush), typeof(Border), null);
        public static readonly BindableProperty BackgroundSizingProperty = PropertyUtils.Register(nameof(BackgroundSizing), typeof(BackgroundSizing), typeof(Border), BackgroundSizing.InnerBorderEdge);
        public static readonly BindableProperty BorderBrushProperty = PropertyUtils.Register(nameof(BorderBrush), typeof(Brush), typeof(Border), null);
        public static readonly BindableProperty BorderThicknessProperty = PropertyUtils.Register(nameof(BorderThickness), typeof(Thickness), typeof(Border), Thickness.Default);
        public static readonly BindableProperty ChildProperty = PropertyUtils.Register(nameof(Child), typeof(StandardUIView), typeof(Border), null);
        public static readonly BindableProperty CornerRadiusProperty = PropertyUtils.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Border), CornerRadius.Default);
        public static readonly BindableProperty PaddingProperty = PropertyUtils.Register(nameof(Padding), typeof(Thickness), typeof(Border), Thickness.Default);
        
        public Brush Background
        {
            get => (Brush) GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        IBrush IBorder.Background
        {
            get => Background;
            set => Background = (Brush) value;
        }
        
        public BackgroundSizing BackgroundSizing
        {
            get => (BackgroundSizing) GetValue(BackgroundSizingProperty);
            set => SetValue(BackgroundSizingProperty, value);
        }
        
        public Brush BorderBrush
        {
            get => (Brush) GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        IBrush IBorder.BorderBrush
        {
            get => BorderBrush;
            set => BorderBrush = (Brush) value;
        }
        
        public Thickness BorderThickness
        {
            get => (Thickness) GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        
        public StandardUIView Child
        {
            get => (StandardUIView) GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }
        IUIElement IBorder.Child
        {
            get => Child;
            set => Child = (StandardUIView) value;
        }
        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}
