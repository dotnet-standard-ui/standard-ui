// This file is generated from ITextBlock.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Maui.Media;
using Microsoft.StandardUI.Text;
using Microsoft.StandardUI.Maui.Text;
using Microsoft.StandardUI.Controls;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;
using Brush = Microsoft.StandardUI.Maui.Media.Brush;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class TextBlock : BuiltInUIElement, ITextBlock
    {
        public static readonly BindableProperty ForegroundProperty = PropertyUtils.Register(nameof(Foreground), typeof(Brush), typeof(TextBlock), null);
        public static readonly BindableProperty TextProperty = PropertyUtils.Register(nameof(Text), typeof(string), typeof(TextBlock), "");
        public static readonly BindableProperty FontFamilyProperty = PropertyUtils.Register(nameof(FontFamily), typeof(FontFamily), typeof(TextBlock), "");
        public static readonly BindableProperty FontStyleProperty = PropertyUtils.Register(nameof(FontStyle), typeof(FontStyle), typeof(TextBlock), FontStyle.Normal);
        public static readonly BindableProperty FontWeightProperty = PropertyUtils.Register(nameof(FontWeight), typeof(FontWeightMaui), typeof(TextBlock), FontWeightMaui.Default);
        public static readonly BindableProperty FontSizeProperty = PropertyUtils.Register(nameof(FontSize), typeof(double), typeof(TextBlock), 11.0);
        public static readonly BindableProperty FontStretchProperty = PropertyUtils.Register(nameof(FontStretch), typeof(FontStretch), typeof(TextBlock), FontStretch.Normal);
        public static readonly BindableProperty TextAlignmentProperty = PropertyUtils.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(TextBlock), TextAlignment.Left);
        
        public Brush Foreground
        {
            get => (Brush) GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        IBrush ITextBlock.Foreground
        {
            get => Foreground;
            set => Foreground = (Brush) value;
        }
        
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        
        public FontFamily FontFamily
        {
            get => (FontFamily) GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        
        public FontStyle FontStyle
        {
            get => (FontStyle) GetValue(FontStyleProperty);
            set => SetValue(FontStyleProperty, value);
        }
        
        public FontWeightMaui FontWeight
        {
            get => (FontWeightMaui) GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }
        FontWeight ITextBlock.FontWeight
        {
            get => FontWeight.FontWeight;
            set => FontWeight = new FontWeightMaui(value);
        }
        
        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        
        public FontStretch FontStretch
        {
            get => (FontStretch) GetValue(FontStretchProperty);
            set => SetValue(FontStretchProperty, value);
        }
        
        public TextAlignment TextAlignment
        {
            get => (TextAlignment) GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
    }
}
