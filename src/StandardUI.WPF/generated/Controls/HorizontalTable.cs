// This file is generated from IHorizontalTable.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using DependencyProperty = System.Windows.DependencyProperty;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class HorizontalTable : GridBase, IHorizontalTable
    {
        public static readonly DependencyProperty RowDefinitionsProperty = PropertyUtils.Register(nameof(RowDefinitions), typeof(UICollection<IRowDefinition>), typeof(HorizontalTable), null);
        public static readonly DependencyProperty ColumnsProperty = PropertyUtils.Register(nameof(Columns), typeof(UIElementCollection<Column,IColumn>), typeof(HorizontalTable), null);
        
        private UICollection<IRowDefinition> _rowDefinitions;
        private UIElementCollection<Column,IColumn> _columns;
        
        public HorizontalTable()
        {
            _rowDefinitions = new UICollection<IRowDefinition>(this);
            SetValue(RowDefinitionsProperty, _rowDefinitions);
            _columns = new UIElementCollection<Column,IColumn>(this);
            SetValue(ColumnsProperty, _columns);
        }
        
        public UICollection<IRowDefinition> RowDefinitions => _rowDefinitions;
        IUICollection<IRowDefinition> IHorizontalTable.RowDefinitions => RowDefinitions;
        
        public UIElementCollection<Column,IColumn> Columns => _columns;
        IUICollection<IColumn> IHorizontalTable.Columns => Columns.ToStandardUIElementCollection();
        
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) =>
            HorizontalTableLayoutManager.Instance.MeasureOverride(this, constraint.ToStandardUISize()).ToWpfSize();
        
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) =>
            HorizontalTableLayoutManager.Instance.ArrangeOverride(this, arrangeSize.ToStandardUISize()).ToWpfSize();
    }
}
