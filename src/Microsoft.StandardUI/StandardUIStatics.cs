using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI
{
    public static class StandardUIStatics
    {
        /*** UI Elements ***/

        public static ICanvas Canvas() => Factory.CreateCanvas();
        public static IStackPanel StackPanel() => Factory.CreateStackPanel();
        public static IVerticalStack VerticalStack() => Factory.CreateVerticalStack();
        public static IHorizontalStack HorizontalStack() => Factory.CreateHorizontalStack();
        public static IGrid Grid() => Factory.CreateGrid();
        public static IRowDefinition RowDefinition() => Factory.CreateRowDefinition();
        public static IColumnDefinition ColumnDefinition() => Factory.CreateColumnDefinition();
        public static IEllipse Ellipse() => Factory.CreateEllipse();
        public static ILine Line() => Factory.CreateLine();
        public static IPath Path() => Factory.CreatePath();
        public static IPolygon Polygon() => Factory.CreatePolygon();
        public static IPolyline Polyline() => Factory.CreatePolyline();
        public static IRectangle Rectangle() => Factory.CreateRectangle();

        public static ITextBlock TextBlock() => Factory.CreateTextBlock();

        /*** Media objects ***/

        public static ISolidColorBrush SolidColorBrush() => Factory.CreateSolidColorBrush();
        public static ISolidColorBrush SolidColorBrush(Color color) => Factory.CreateSolidColorBrush().Color(color);
        public static ILinearGradientBrush LinearGradientBrush() => Factory.CreateLinearGradientBrush();
        public static IRadialGradientBrush RadialGradientBrush() => Factory.CreateRadialGradientBrush();

        /*** Constants **/

        public static GridLength Star => GridLength.Default;

        internal static IStandardUIFactory Factory => HostEnvironment.Factory;
    }
}
