using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using System;

namespace Microsoft.StandardUI.WinUI
{
    /// <summary>
    /// This is the base for predefined Standard UI controls. 
    /// </summary>
    public class BuiltInUIElement : FrameworkElement, IUIElement
    {
        private StandardUIFrameworkElementHelper _helper = new();

        void IUIElement.Measure(Size availableSize)
        {
            Measure(availableSize.ToWindowsFoundationSize());
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToWindowsFoundationRect());
        }

        Size IUIElement.DesiredSize => SizeExtensions.ToStandardUISize(DesiredSize);

        double IUIElement.ActualX => throw new NotImplementedException();

        double IUIElement.ActualY => throw new NotImplementedException();

        Thickness IUIElement.Margin
        {
            get => Margin.ToStandardUIThickness();
            set => Margin = value.ToWinUIThickness();
        }

        HorizontalAlignment IUIElement.HorizontalAlignment
        {
            get => HorizontalAlignmentExtensions.FromWinUIHorizontalAlignment(HorizontalAlignment);
            set => HorizontalAlignment = value.ToWinUIHorizontalAlignment();
        }

        VerticalAlignment IUIElement.VerticalAlignment
        {
            get => VerticalAlignmentExtensions.ToStandardUIVerticalAlignment(VerticalAlignment);
            set => VerticalAlignment = value.ToWinUIVerticalAlignment();
        }

        FlowDirection IUIElement.FlowDirection
        {
            get => FlowDirectionExtensions.ToStandardUIFlowDirection(FlowDirection);
            set => FlowDirection = value.ToWinUIFlowDirection();
        }

        // TODO: Error if appropriate when set to Visibility.Hidden
        bool IUIElement.Visible
        {
#pragma warning disable CA1033 // Interface methods should be callable by child types
            get => Visibility != Visibility.Collapsed;
            set => Visibility = value ? Visibility.Visible : Visibility.Collapsed;
#pragma warning restore CA1033 // Interface methods should be callable by child types
        }

#if false
        protected override void OnRender(DrawingContext drawingContextWpf)
        {
            base.OnRender(drawingContextWpf);

            if (Visibility != Visibility.Visible)
                return;

            IVisual visual;
            using (IDrawingContext drawingContext = visualEnvironment.CreateDrawingContext(this)) {
                Draw(drawingContext);
                visual = drawingContext.End();
            }

            _helper.OnRender(visual, Width, Height, drawingContextWpf);
        }
#endif

        private void Rebuild()
        {
#if false
            if (_buildContent != null)
            {
                RemoveVisualChild(_buildContent);
                RemoveLogicalChild(_buildContent);
                _buildContent = null;
            }

            _buildContent = (BuiltInUIElement?)_implementation.Build();

            if (_buildContent != null)
            {
                AddVisualChild(_buildContent);
                AddLogicalChild(_buildContent);
            }
#endif
        }

        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        public void ClearValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            ClearValue(dependencyProperty);
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
    }
}
