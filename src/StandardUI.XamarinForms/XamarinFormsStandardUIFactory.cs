using System;
using System.Collections.Generic;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using Microsoft.StandardUI.XamarinForms.Controls;
using Microsoft.StandardUI.XamarinForms.Media;
using Microsoft.StandardUI.XamarinForms.Shapes;

namespace Microsoft.StandardUI.XamarinForms
{
    internal class XamarinFormsStandardUIFactory : IStandardUIFactory
    {
        /*** UI Elements ***/

        public ICanvas CreateCanvas() => new Canvas();
        public ICanvasAttached CanvasAttached => Controls.CanvasAttached.Instance;
        public IStackPanel CreateStackPanel() => new StackPanel();
        public IGrid CreateGrid() => new Grid();

        public IEllipse CreateEllipse() => new Ellipse();
        public ILine CreateLine() => new Line();
        public IPath CreatePath() => new Path();
        public IPolygon CreatePolygon() => new Polygon();
        public IPolyline CreatePolyline() => new Polyline();
        public IRectangle CreateRectangle() => new Rectangle();

        public ITextBlock CreateTextBlock() => new TextBlock();

        /*** Media objects ***/

        public ISolidColorBrush CreateSolidColorBrush() => new SolidColorBrush();
        public ILinearGradientBrush CreateLinearGradientBrush() => new LinearGradientBrush();
        public IRadialGradientBrush CreateRadialGradientBrush() => new RadialGradientBrush();

        public IArcSegment CreateArcSegment(in Point point, in Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection) => throw new NotImplementedException();
        public IBezierSegment CreateBezierSegment(in Point point1, in Point point2, in Point point3) => throw new NotImplementedException();
        public ILineSegment CreateLineSegment(in Point point) => throw new NotImplementedException();
        public IPathFigure CreatePathFigure(IEnumerable<IPathSegment> segments, Point startPoint, bool isClosed, bool isFilled) => throw new NotImplementedException();
        public IPathGeometry CreatePathGeometry(ITransform? transform, IEnumerable<IPathFigure> figures, FillRule fillRule) => throw new NotImplementedException();
        public IPolyBezierSegment CreatePolyBezierSegment(Points points) => throw new NotImplementedException();
        public IPolyLineSegment CreatePolyLineSegment(Points points) => throw new NotImplementedException();
        public IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(Points points) => throw new NotImplementedException();
        public IQuadraticBezierSegment CreateQuadraticBezierSegment(in Point point1, in Point point2) => throw new NotImplementedException();

        /*** Infrastructure objects ***/

        public IUIPropertyMetadata CreatePropertyMetadata(object defaultValue) => throw new NotImplementedException();
        public IUIPropertyMetadata CreatePropertyMetadata(object defaultValue, UIPropertyChangedCallback propertyChangedCallback) => throw new NotImplementedException();
        public IUIProperty RegisterUIProperty(string name, Type propertyType, Type ownerType, IUIPropertyMetadata typeMetadata) => throw new NotImplementedException();
    }
}
