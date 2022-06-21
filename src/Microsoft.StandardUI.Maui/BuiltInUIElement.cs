using Microsoft.Maui.Controls;
using Microsoft.StandardUI.Maui;
using System;

namespace Microsoft.StandardUI.Maui
{
    /// <summary>
    /// This is the base for predefined Standard UI controls. 
    /// </summary>
    public class BuiltInUIElement : View, IUIElement
    {
        void IUIElement.Measure(Size availableSize)
        {
            Microsoft.Maui.SizeRequest sizeRequest = this.Measure(availableSize.Width, availableSize.Height, MeasureFlags.None);

            this.HorizontalOptions = this.HorizontalOptions;
        }

        void IUIElement.Arrange(Rect finalRect)
        {
            Arrange(finalRect.ToMauiRect());
        }

        Size IUIElement.DesiredSize => SizeExtensions.ToStandardUISize(DesiredSize);

        double IUIElement.ActualX => throw new NotImplementedException();

        double IUIElement.ActualY => throw new NotImplementedException();

        HorizontalAlignment IUIElement.HorizontalAlignment
        {
            get => HorizontalOptions.Alignment.ToStandardUIHorizontalAlignment();
            set => HorizontalOptions = new LayoutOptions(value.ToMauiLayoutAlignment(), HorizontalOptions.Expands);
        }

        VerticalAlignment IUIElement.VerticalAlignment
        {
            get => VerticalOptions.Alignment.ToStandardUIVerticalAlignment();
            set => VerticalOptions = new LayoutOptions(value.ToMauiLayoutAlignment(), VerticalOptions.Expands);
        }

        FlowDirection IUIElement.FlowDirection
        {
            get => FlowDirection.ToStandardUIFlowDirection();
            set => FlowDirection = value.ToMauiFlowDirection();
        }

        // TODO: Error if appropriate when set to Visibility.Hidden
        bool IUIElement.Visible
        {
            get => IsVisible;
            set => IsVisible = value;
        }

        double IUIElement.Width
        {
            get => WidthRequest;
            set => WidthRequest = value;
        }

        double IUIElement.MinWidth
        {
            get => MinimumWidthRequest;
            set => MinimumWidthRequest = value;
        }

        double IUIElement.MaxWidth
        {
            get => MaximumWidthRequest;
            set => MaximumWidthRequest = value;
        }

        double IUIElement.Height
        {
            get => HeightRequest;
            set => HeightRequest = value;
        }

        double IUIElement.MinHeight
        {
            get => MinimumHeightRequest;
            set => MinimumHeightRequest = value;
        }

        double IUIElement.MaxHeight
        {
            get => MaximumHeightRequest;
            set => MaximumHeightRequest = value;
        }

        double IUIElement.ActualWidth => Width;

        double IUIElement.ActualHeight => Height;

#if TODO
        protected override void OnRender(DrawingContext drawingContextWpf)
        {
            base.OnRender(drawingContextWpf);

            if (Visibility != Visibility.Visible)
                return;

            IVisualFramework visualFramework = HostEnvironment.VisualFramework;

            Rect cullingRect = new Rect(0, 0, 200, 200);

            IVisual? visual;
            using (IDrawingContext drawingContext = visualFramework.CreateDrawingContext(this)) {
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
#endif

        public virtual void Draw(IDrawingContext visualizer)
        {
        }

        object? IUIObject.GetValue(IUIProperty property) => GetValue(((UIProperty) property).BindableProperty);
        object? IUIObject.ReadLocalValue(IUIProperty property) => throw new NotSupportedException("ReadLocalValue isn't currently supported");   // ReadLocalValue(((UIProperty)property).BindableProperty);
        void IUIObject.SetValue(IUIProperty property, object? value) => SetValue(((UIProperty)property).BindableProperty, value);
        void IUIObject.ClearValue(IUIProperty property) => ClearValue(((UIProperty)property).BindableProperty);

#if TODO
        void ILogicalParent.AddLogicalChild(object child) => AddLogicalChild(child);
        void ILogicalParent.RemoveLogicalChild(object child) => RemoveLogicalChild(child);
#endif
    }
}
