// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class Grid : Panel, IGrid
    {
        public static readonly BindableProperty ColumnDefinitionsProperty = PropertyUtils.Register(nameof(ColumnDefinitions), typeof(UICollection<IColumnDefinition>), typeof(Grid), null);
        public static readonly BindableProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(UICollection<IRowDefinition>), typeof(Grid), null);
        public static readonly BindableProperty ColumnSpacingProperty = PropertyUtils.Register(nameof(ColumnSpacing), typeof(double), typeof(Grid), 0.0);
        public static readonly BindableProperty RowSpacingProperty = PropertyUtils.Register(nameof(RowSpacing), typeof(double), typeof(Grid), 0.0);
        public static readonly BindableProperty RowProperty = PropertyUtils.RegisterAttached("Row", typeof(int), typeof(Microsoft.Maui.Controls.View), 0);
        public static readonly BindableProperty ColumnProperty = PropertyUtils.RegisterAttached("Column", typeof(int), typeof(Microsoft.Maui.Controls.View), 0);
        public static readonly BindableProperty RowSpanProperty = PropertyUtils.RegisterAttached("RowSpan", typeof(int), typeof(Microsoft.Maui.Controls.View), 1);
        public static readonly BindableProperty ColumnSpanProperty = PropertyUtils.RegisterAttached("ColumnSpan", typeof(int), typeof(Microsoft.Maui.Controls.View), 1);
        
        public static int GetRow(Microsoft.Maui.Controls.View element) => (int) element.GetValue(RowProperty);
        public static void SetRow(Microsoft.Maui.Controls.View element, int value) => element.SetValue(RowProperty, value);
        
        public static int GetColumn(Microsoft.Maui.Controls.View element) => (int) element.GetValue(ColumnProperty);
        public static void SetColumn(Microsoft.Maui.Controls.View element, int value) => element.SetValue(ColumnProperty, value);
        
        public static int GetRowSpan(Microsoft.Maui.Controls.View element) => (int) element.GetValue(RowSpanProperty);
        public static void SetRowSpan(Microsoft.Maui.Controls.View element, int value) => element.SetValue(RowSpanProperty, value);
        
        public static int GetColumnSpan(Microsoft.Maui.Controls.View element) => (int) element.GetValue(ColumnSpanProperty);
        public static void SetColumnSpan(Microsoft.Maui.Controls.View element, int value) => element.SetValue(ColumnSpanProperty, value);
        
        private UICollection<IColumnDefinition> _columnDefinitions;
        private UICollection<IRowDefinition> _rowDefinitions;
        
        public Grid()
        {
            _columnDefinitions = new UICollection<IColumnDefinition>(this);
            SetValue(ColumnDefinitionsProperty, _columnDefinitions);
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
        }
        
        public UICollection<IColumnDefinition> ColumnDefinitions => _columnDefinitions;
        IUICollection<IColumnDefinition> IGrid.ColumnDefinitions => ColumnDefinitions;
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IGrid.RowDefinitions => RowDefinitions;
        
        public double ColumnSpacing
        {
            get => (double) GetValue(ColumnSpacingProperty);
            set => SetValue(ColumnSpacingProperty, value);
        }
        
        public double RowSpacing
        {
            get => (double) GetValue(RowSpacingProperty);
            set => SetValue(RowSpacingProperty, value);
        }
    }
}
