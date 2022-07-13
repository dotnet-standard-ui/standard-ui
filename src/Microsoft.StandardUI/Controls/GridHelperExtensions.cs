using static Microsoft.StandardUI.StandardUIStatics;

namespace Microsoft.StandardUI.Controls
{
    public static class GridHelperExtensions
    {
        public static T RowDefinitions<T>(this T grid, params GridLength[] rowHeights) where T : IGrid
        {
            int length = rowHeights.Length;
            var rowDefinitions = new IRowDefinition[length];
            for (int i = 0; i < length; ++i)
            {
                rowDefinitions[i] = RowDefinition().Height(rowHeights[i]);
            }

            grid.RowDefinitions.Set(rowDefinitions);
            return grid;
        }

        public static T ColumnDefinitions<T>(this T grid, params GridLength[] columnWidths) where T : IGrid
        {
            int length = columnWidths.Length;
            var columnDefinitions = new IColumnDefinition[length];
            for (int i = 0; i < length; ++i)
            {
                columnDefinitions[i] = ColumnDefinition().Width(columnWidths[i]);
            }

            grid.ColumnDefinitions.Set(columnDefinitions);
            return grid;
        }

        public static T _<T>(this T grid, params IUIElement[] children) where T : IGrid =>
            grid.Children(children);

        public static T GridCell<T>(this T uiElement, int row, int column) where T : IUIElement =>
            uiElement.GridRow(row).GridColumn(column);
    }
}
