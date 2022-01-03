using System;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf
{
    /// <summary>
    /// This is the base for predefined Standard UI controls. 
    /// </summary>
    public class BuiltInUIElement : FrameworkElement, IUIElement, ILogicalParent
    {
        private StandardUIFrameworkElementHelper _helper = new();

        void IUIElement.Measure(Size availableSize)
        {
            Measure(availableSize.ToWpfSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWpfRect());
        }

        Size IUIElement.DesiredSize => SizeExtensions.ToStandardUISize(DesiredSize);

        double IUIElement.ActualX => throw new NotImplementedException();

        double IUIElement.ActualY => throw new NotImplementedException();

        HorizontalAlignment IUIElement.HorizontalAlignment
        {
            get => HorizontalAlignmentExtensions.ToStandardUIHorizontalAlignment(HorizontalAlignment);
            set => HorizontalAlignment = value.ToWpfHorizontalAlignment();
        }

        VerticalAlignment IUIElement.VerticalAlignment
        {
            get => VerticalAlignmentExtensions.FromWpfVerticalAlignment(VerticalAlignment);
            set => VerticalAlignment = value.ToWpfVerticalAlignment();
        }

        FlowDirection IUIElement.FlowDirection
        {
            get => FlowDirectionExtensions.ToStandardUIFlowDirection(FlowDirection);
            set => FlowDirection = value.ToWpfFlowDirection();
        }

        // TODO: Error if appropriate when set to Visibility.Hidden
        bool IUIElement.IsVisible
        {
            get => Visibility != Visibility.Collapsed;
            set => Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnRender(DrawingContext drawingContextWpf)
        {
            base.OnRender(drawingContextWpf);

            if (Visibility != Visibility.Visible)
                return;

            IVisualEnvironment visualEnvironment = StandardUIEnvironment.VisualEnvironment;

            Rect cullingRect = new Rect(0, 0, 200, 200);

            IVisual? visual;
            using (IDrawingContext drawingContext = visualEnvironment.CreateDrawingContext(this)) {
                Draw(drawingContext);
                visual = drawingContext.Close();
            }

            if (visual != null)
            {
                _helper.OnRender(visual, Width, Height, drawingContextWpf);
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        public object GetValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return GetValue(dependencyProperty);
        }

        public object ReadLocalValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return ReadLocalValue(dependencyProperty);
        }

        public void SetValue(IUIProperty property, object value)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            SetValue(dependencyProperty, value);
        }

        void ILogicalParent.AddLogicalChild(object child) => this.AddLogicalChild(child);

        void ILogicalParent.RemoveLogicalChild(object child) => this.RemoveLogicalChild(child);
    }
}