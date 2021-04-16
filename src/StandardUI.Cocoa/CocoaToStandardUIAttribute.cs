using System;
namespace Microsoft.StandardUI.Cocoa
{
    /// <summary>
    /// Indicates the class should be projected from Cocoa to StandardUI
    /// </summary>
    /// <remarks>
    /// The given class must inherit from NSView and StandardUI.Cocoa.Generator must be referenced
    /// from your assembly.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CocoaToStandardUIAttribute : Attribute
    {
        public CocoaToStandardUIAttribute(string standardUITypeName)
        {
        }

        public string StandardUITypeName { get; }
    }
}
