using System.ComponentModel;

namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IUIElement : IUIObject
    {
        /// <summary>
        /// The width of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.NaN)]
        double Width { get; set; }

        /// <summary>
        /// The minimum width of the object, in pixels. The default is 0. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(0.0)]
        public double MinWidth { get; set; }

        /// <summary>
        /// The maximum width of the object, in pixels. The default is PositiveInfinity. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        public double MaxWidth { get; set; }

        /// <summary>
        /// The horizontal alignment characteristics that are applied to this UIElement when it is composed in a layout parent, such as a panel or items control.
        /// </summary>
        [DefaultValue(HorizontalAlignment.Stretch)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// The height of the object, in pixels. The default is NaN. Except for the special NaN value, this value must be equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.NaN)]
        double Height { get; set; }

        /// <summary>
        /// The minimum height of the object, in pixels. The default is 0. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(0.0)]
        public double MinHeight { get; set; }

        /// <summary>
        /// The maximum height of the object, in pixels. The default is PositiveInfinity. This value can be any value equal to or greater than 0.
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        public double MaxHeight { get; set; }

        /// <summary>
        /// The outer margin of the element, the distance between it and its adjacent elements
        /// </summary>
        public Thickness Margin { get; set; }

        /// <summary>
        /// The Vertical alignment characteristics that are applied to this UIElement when it is composed in a layout parent, such as a panel or items control.
        /// </summary>
        [DefaultValue(VerticalAlignment.Stretch)]
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// THe direction in which text and other UI elements flow within any parent element that controls their layout. This property can be set to
        /// either LeftToRight or RightToLeft. Setting FlowDirection to RightToLeft on any element sets the alignment to the right, the reading order
        /// to right-to-left and the layout of the control to flow from right to left.
        /// </summary>
        [DefaultValue(FlowDirection.LeftToRight)]
        public FlowDirection FlowDirection { get; set; }

        /// <summary>
        /// Gets the size that this UIElement computed during the measure pass of the layout process.
        /// </summary>
        public Size DesiredSize { get; }

        /// <summary>
        /// Updates the DesiredSize of a UIElement. Typically, objects that implement custom layout for their layout children call this method from
        /// their own MeasureOverride implementations to form a recursive layout update.
        /// </summary>
        /// <param name="availableSize">The available space that a parent can allocate to a child object. A child object can request a larger space
        /// than what is available; the provided size might be accommodated if scrolling or other resize behavior is possible in that particular container.
        /// </param>
        public void Measure(Size availableSize);

        /// <summary>
        /// Positions child objects and determines a size for a UIElement. Parent objects that implement custom layout for their child elements should
        /// call this method from their layout override implementations to form a recursive layout update.
        /// </summary>
        /// <param name="finalRect">The final size that the parent computes for the child in layout, provided as a Rect value.</param>
        public void Arrange(Rect finalRect);

        /// <summary>
        /// Gets the X position of this UIElement, relative to its parent, computed during the arrange pass of the layout process
        /// </summary>
        [DefaultValue(0)]
        public double ActualX { get; }

        /// <summary>
        /// Gets the Y position of this UIElement, relative to its parent, computed during the arrange pass of the layout process
        /// </summary>
        [DefaultValue(0)]
        public double ActualY { get; }

        /// <summary>
        /// Gets the rendered width of a UIElement.
        /// </summary>
        [DefaultValue(0)]
        public double ActualWidth { get; }

        /// <summary>
        /// Gets the rendered height of a UIElement
        /// </summary>
        [DefaultValue(0)]
        public double ActualHeight { get; }

        /// <summary>
        /// The visibility of a UIElement. A UIElement that is not visible is not rendered, does not take up space in the
        /// layout, and cannot receive focus or input events.
        /// </summary>
        [DefaultValue(true)]
        public bool Visible { get; set; }
    }
}
