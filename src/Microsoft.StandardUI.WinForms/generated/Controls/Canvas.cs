// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.DefaultImplementations;
using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.WinForms.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly AttachedUIProperty LeftProperty = new AttachedUIProperty("Left", 0.0);
        public static readonly AttachedUIProperty TopProperty = new AttachedUIProperty("Top", 0.0);
        
        public static double GetLeft(System.Windows.Forms.Control element) => (double) AttachedPropertiesValues.GetValue(element, LeftProperty);
        public static void SetLeft(System.Windows.Forms.Control element, double value) => AttachedPropertiesValues.SetValue(element, LeftProperty, value);
        
        public static double GetTop(System.Windows.Forms.Control element) => (double) AttachedPropertiesValues.GetValue(element, TopProperty);
        public static void SetTop(System.Windows.Forms.Control element, double value) => AttachedPropertiesValues.SetValue(element, TopProperty, value);
    }
}
