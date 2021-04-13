namespace Microsoft.StandardUI.Media
{
    /// <summary>
    /// Describes the shape at the end of a line or segment.
    /// </summary>
    public enum PenLineCap
    {
        /// <summary>
        /// A cap that does not extend past the last point of the line. Comparable to no line cap.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// A rectangle that has a height equal to the line thickness and a length equal to half the line thickness.
        /// </summary>
        Square = 1,

        /// <summary>
        /// A semicircle that has a diameter equal to the line thickness.
        /// </summary>
        Round = 2
    }
}
