// This file is generated from IStack.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using BindableProperty = Microsoft.Maui.Controls.BindableProperty;

namespace Microsoft.StandardUI.Maui.Controls
{
    public class Stack : StackBase, IStack
    {
        public static readonly BindableProperty OrientationProperty = PropertyUtils.Register(nameof(Orientation), typeof(Orientation), typeof(Stack), Orientation.Vertical);
        
        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
    }
}
