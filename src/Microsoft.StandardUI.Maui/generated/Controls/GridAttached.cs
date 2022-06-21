// This file is generated from IGrid.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class GridAttached : IGridAttached
    {
        public static GridAttached Instance = new GridAttached();
        
        public int GetRow(IUIElement element) => Grid.GetRow(element.ToView());
        public void SetRow(IUIElement element, int value) => Grid.SetRow(element.ToView(), value);
        
        public int GetColumn(IUIElement element) => Grid.GetColumn(element.ToView());
        public void SetColumn(IUIElement element, int value) => Grid.SetColumn(element.ToView(), value);
        
        public int GetRowSpan(IUIElement element) => Grid.GetRowSpan(element.ToView());
        public void SetRowSpan(IUIElement element, int value) => Grid.SetRowSpan(element.ToView(), value);
        
        public int GetColumnSpan(IUIElement element) => Grid.GetColumnSpan(element.ToView());
        public void SetColumnSpan(IUIElement element, int value) => Grid.SetColumnSpan(element.ToView(), value);
    }
}
